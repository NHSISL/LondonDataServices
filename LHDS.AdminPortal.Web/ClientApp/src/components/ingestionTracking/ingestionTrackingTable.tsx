import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
import { IngestionTrackingHomeView } from "../../models/ingestionTrackings/ingestionTrackingHomeView";
import { LookupView } from "../../models/views/components/lookups/lookupView";
import { ingestionTrackingHomeViewService } from "../../services/views/ingestionTrackingHomeViewService";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";
import SearchBase from "../bases/inputs/SearchBase";
import InfiniteScroll from "../bases/pagers/InfiniteScroll";
import InfiniteScrollLoader from "../bases/pagers/InfiniteScroll.Loader";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import IngestionTrackingRow from "./ingestionTrackingRow";

type IngestionTrackingTableProps = {};

const IngestionTrackingTable: FunctionComponent<IngestionTrackingTableProps> = (props) => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [selectedSupplier, setSelectedSupplier] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");

    const {
        mappedIngestionTrackings: ingestionTrackingsRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
    } = ingestionTrackingHomeViewService.useGetAllIngestionTrackings(
        debouncedTerm,
        selectedSupplier
    );

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    };

    const handleDebounce = useMemo(
        () =>
            debounce((value: string) => {
                setDebouncedTerm(value);
            }, 500),
        []
    );

    const supplierOptions: Array<LookupView> = [
        { id: "", name: "Please select..." },
        ...(data?.supplierOptions || []).map((supplier: LookupView) => {
            return { id: supplier.id.toString(), name: supplier.name || "" };
        }),
    ];

    const handleSupplierChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedSupplier(event.target.value);
    };

    const hasNoMorePages = () => {
        return !isLoading && data?.pages.at(-1)?.nextPage === undefined;
    };

    return (
        <div className="infiniteScollContainer">
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>Ingestion Trackings</CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll
                            loading={isLoading}
                            hasNextPage={!!data?.hasNextPage}
                            loadMore={fetchNextPage}
                        >
                            <div className="filter-container">
                                <div className="filter-item">
                                    <SearchBase
                                        id="search"
                                        label="Search IngestionTrackings"
                                        value={searchTerm}
                                        onChange={(e) => {
                                            handleSearchChange(e.currentTarget.value);
                                        }}
                                    />
                                </div>
                                <div className="filter-item">
                                    <label htmlFor="supplier-filter">Filter by Supplier: &nbsp;</label>
                                    <select
                                        id="supplier-filter"
                                        value={selectedSupplier}
                                        onChange={(e) =>
                                            setSelectedSupplier(e.currentTarget.value)
                                        }
                                    >
                                        {supplierOptions.map((option) => (
                                            <option key={option.id} value={option.id}>
                                                {option.name}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                            </div>

                            <TableBase>
                                <TableBaseTbody>
                                    {ingestionTrackingsRetrieved?.map
                                        ((ingestionTrackingHomeView: IngestionTrackingHomeView) => (
                                            <IngestionTrackingRow
                                                key={ingestionTrackingHomeView.id}
                                                ingestionTracking={ingestionTrackingHomeView}
                                            />
                                        ))}
                                    <tr>
                                        <td colSpan={3} className="text-center">
                                            <InfiniteScrollLoader
                                                loading={isLoading || isFetchingNextPage}
                                                spinner={<SpinnerBase />}
                                                noMorePages={hasNoMorePages()}
                                                noMorePagesMessage={<>---No more IngestionTrackings---</>}
                                            />
                                        </td>
                                    </tr>
                                </TableBaseTbody>
                            </TableBase>
                        </InfiniteScroll>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
};

export default IngestionTrackingTable;
