import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
import { IngestionTrackingHomeView } from "../../models/ingestionTrackings/ingestionTrackingHomeView";
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
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import { Row } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDatabase, faRefresh } from "@fortawesome/free-solid-svg-icons";
import IngestionFilterModal from "./ingestionTrackingFilter"; 
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import { emisLandingService } from "../../services/foundations/emisLandingService";
import { toastError, toastSuccess } from "../../brokers/toastBroker";
import { lookupViewService } from "../../services/views/lookups/lookupViewService";

type IngestionTrackingTableProps = {};

const IngestionTrackingTable: FunctionComponent<IngestionTrackingTableProps> = (props) => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [debouncedSupplierTerm, setDebouncedSupplierTerm] = useState<string>("");
    const [showSpinner, setShowSpinner] = useState(false);
    const [showModal, setShowModal] = useState(false);
    let { mappedSuppliers: suppliersRetrieved } = lookupViewService.useGetSupplierList("");

    const {
        mappedIngestionTrackings: ingestionTrackingsRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
        refetch
    } = ingestionTrackingHomeViewService.useGetAllIngestionTrackings(
        debouncedTerm, debouncedSupplierTerm
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

    const handleSupplierDebounce = useMemo(
        () =>
            debounce((value: string) => {
                setDebouncedSupplierTerm(value);
            }, 500),
        []
    );

    const updateEmisLanding = emisLandingService.useModifyEmisLanding();
    const handleReDecrypt = (ingestionTracking: IngestionTracking) => {
        updateEmisLanding.updateIngestionTracking(ingestionTracking)
            .then(() => {
                toastSuccess("Ingestion Tracking Queued for Decrypt")
            })
            .catch(e => {
                toastError("error")
            });
    };

    const handleFilter = (supplier: SupplierView) => {
        setSearchTerm(debouncedTerm);
        handleSupplierDebounce(supplier.id.toString());
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const selectedSupplierId = event.target.value;
        setDebouncedSupplierTerm(selectedSupplierId);
    };

    const hasNoMorePages = () => {
        return !isLoading && data?.pages.at(-1)?.nextPage === undefined;
    };

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
                    <CardBaseTitle> <FontAwesomeIcon icon={faDatabase} className="me-2" /> Ingestion Tracking</CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll loading={isLoading || showSpinner} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
                            <Row className="m-0 p-0">
                                <div className="input-group mb-3 m-0 p-0">
                                    <SearchBase
                                        id="search"
                                        value={searchTerm}
                                        placeholder="Search for ingestion data..."
                                        onChange={(e) => { handleSearchChange(e.currentTarget.value) }} />

                                    {showSpinner ? (
                                        <SpinnerBase />
                                    ) : (
                                            <div className="input-group-append m-0 p-0">
                                                <button className="btn btn-outline-secondary" id="refreshButton" onClick={refreshData}>
                                                <FontAwesomeIcon icon={faRefresh} /> Refresh
                                            </button>
                                        </div>
                                    )}
                                </div>
                            </Row>
                            <span className="d-flex align-items-center">
                                {/* "All" Option */}
                                <div key="all-suppliers" className="form-check me-3">
                                    <input
                                        className="form-check-input"
                                        type="radio"
                                        name="supplier"
                                        id="supplier-all"
                                        value=""
                                        onChange={handleChange}
                                    />
                                    <label className="form-check-label" htmlFor="supplier-all">
                                        All
                                    </label>
                                </div>

                                {/* Filtered Suppliers: Only "EMIS" and "TPP" */}
                                {suppliersRetrieved
                                    .filter((supplier) => supplier.name === "EMIS" || supplier.name === "TPP")
                                    .map((supplier) => (
                                        <div key={supplier.id.toString()} className="form-check me-3">
                                            <input
                                                className="form-check-input"
                                                type="radio"
                                                name="supplier"
                                                id={`supplier-${supplier.id}`}
                                                value={supplier.id.toString()}
                                                onChange={handleChange}
                                            />
                                            <label className="form-check-label" htmlFor={`supplier-${supplier.id}`}>
                                                {supplier.name || ""}
                                            </label>
                                        </div>
                                    ))}
                            </span>
                            <TableBase classes="table-bordered">
                                <TableBaseTbody>
                                    {isLoading || showSpinner ? (
                                        <tr>
                                            <td colSpan={6} className="text-center">
                                                <SpinnerBase />
                                            </td>
                                        </tr>
                                    ) : (
                                        <>
                                            {ingestionTrackingsRetrieved?.map(
                                                (ingestionTrackingHomeView: IngestionTrackingHomeView) => (
                                                    <IngestionTrackingRow
                                                        key={ingestionTrackingHomeView.id}
                                                        ingestionTracking={ingestionTrackingHomeView}
                                                        onReDecrypted={handleReDecrypt}
                                                    />
                                                )
                                            )}
                                            <tr>
                                                <td colSpan={5} className="text-center">
                                                    <InfiniteScrollLoader
                                                        loading={isLoading || isFetchingNextPage}
                                                        spinner={<SpinnerBase />}
                                                        noMorePages={hasNoMorePages()}
                                                        noMorePagesMessage={<>-- No more pages --</>}
                                                    />
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
            {
                showModal && (
                    <IngestionFilterModal onClose={() => setShowModal(false)} onAddFilter={handleFilter} />
                )}
        </div >
    );
};

export default IngestionTrackingTable;