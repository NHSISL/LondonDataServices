import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
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
import { Row } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDatabase, faRefresh } from "@fortawesome/free-solid-svg-icons";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseThead from "../bases/components/Table/TableBase.Thead";
import TableBaseHeader from "../bases/components/Table/TableBase.Header";
import { AddressHomeViewService } from "../../services/views/addresses/addressHomeViewService";
import AddressRow from "./addressRow";
import { Address } from "../../models/addresses/address";

const AddressTable: FunctionComponent = () => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [showSpinner, setShowSpinner] = useState(false);

    const {
        mappedAddresses: addressesRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
        refetch
    } = AddressHomeViewService.useGetAllAddresses(debouncedTerm);

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

    const refreshData = () => {
        setShowSpinner(true);
        setTimeout(() => {
            refetch();
            setTimeout(() => {
                setShowSpinner(false);
            }, 200);
        }, 200);
    };

    return (
        <div className="infiniteScrollContainer">
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        <FontAwesomeIcon icon={faDatabase} className="me-2" /> Addresses (Ordinance Database)
                    </CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll loading={isLoading || showSpinner} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
                            <Row>
                                <div className="input-group mb-3">
                                    <SearchBase
                                        id="search"
                                        value={searchTerm}
                                        placeholder="Search for addresses..."
                                        onChange={(e) => { handleSearchChange(e.currentTarget.value) }} />

                                    {showSpinner ? (
                                        <SpinnerBase />
                                    ) : (
                                        <div className="input-group-append">
                                            <button className="btn btn-outline-secondary" id="refreshButton" onClick={refreshData}>
                                                <FontAwesomeIcon icon={faRefresh} /> Refresh
                                            </button>
                                        </div>
                                    )}
                                </div>
                            </Row>
                            <TableBase classes="table-bordered">
                                <TableBaseThead>
                                    <TableBaseRow>
                                        <TableBaseHeader>UPRN</TableBaseHeader>
                                        <TableBaseHeader>UPSN</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Address</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">PostCode</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">IsProcessing</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">IsSynced</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Actions</TableBaseHeader>
                                    </TableBaseRow>
                                </TableBaseThead>

                                <TableBaseTbody>
                                    {isLoading || showSpinner ? (
                                        <tr>
                                            <td colSpan={6} className="text-center">
                                                <SpinnerBase />
                                            </td>
                                        </tr>
                                    ) : (
                                        <>
                                            {addressesRetrieved?.map(
                                                (address: Address) => (
                                                    <AddressRow
                                                        key={address.id.toString()}
                                                        address={address}
                                                    />
                                                )
                                            )}
                                            <tr>
                                                <td colSpan={5} className="text-center">
                                                    <InfiniteScrollLoader
                                                        loading={isLoading || isFetchingNextPage}
                                                        spinner={<SpinnerBase />}
                                                        noMorePages={!hasNextPage}
                                                        noMorePagesMessage={<>-- No more pages --</>}
                                                        totalPages={data?.pages.length} />
                                                </td>
                                            </tr>
                                        </>
                                    )}
                                </TableBaseTbody>
                            </TableBase>
                        </InfiniteScroll>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
};

export default AddressTable;
