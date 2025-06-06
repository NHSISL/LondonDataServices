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
import { faDatabase, faFilter, faRefresh } from "@fortawesome/free-solid-svg-icons";
import IngestionFilterModal from "./ingestionTrackingFilter";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import { emisLandingService } from "../../services/foundations/emisLandingService";
import { toastError, toastSuccess } from "../../brokers/toastBroker";

type IngestionTrackingTableProps = {};

const IngestionTrackingTable: FunctionComponent<IngestionTrackingTableProps> = (props) => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [debouncedSupplierTerm, setDebouncedSupplierTerm] = useState<string>("");
    const [selectedSupplierId, setSelectedSupplierId] = useState<string>("");
    const [selectedDecryptedFilter, setSelectedDecryptedFilter] = useState<boolean | undefined>(undefined);
    const [selectedBatchCompleteFilter, setSelectedBatchCompleteFilter] = useState<boolean | undefined>(undefined);
    const [selectedProcessingFilter, setSelectedProcessingFilter] = useState<boolean | undefined>(undefined);
    const [selectedDownloadedFilter, setSelectedDownloadedFilter] = useState<boolean | undefined>(undefined);
    const [showSpinner, setShowSpinner] = useState(false);
    const [showModal, setShowModal] = useState(false);
    const [showSidePanel, setShowSidePanel] = useState(false); // 👈 Side panel toggle state

    const decryptedFilterParam = selectedDecryptedFilter === undefined ? "" : selectedDecryptedFilter.toString();
    const downloadedFilterParam = selectedDownloadedFilter === undefined ? "" : selectedDownloadedFilter.toString();
    const batchCompleteFilterParam = selectedBatchCompleteFilter === undefined ? "" : selectedBatchCompleteFilter.toString();
    const processingFilterParam = selectedProcessingFilter === undefined ? "" : selectedProcessingFilter.toString();

    const isFilterActive =
        selectedSupplierId !== "" ||
        selectedDecryptedFilter !== undefined ||
        selectedDownloadedFilter !== undefined ||
        selectedBatchCompleteFilter !== undefined ||
        selectedProcessingFilter !== undefined;

    const {
        mappedIngestionTrackings: ingestionTrackingsRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
        refetch
    } = ingestionTrackingHomeViewService.useGetAllIngestionTrackings(
        debouncedTerm,
        debouncedSupplierTerm,
        decryptedFilterParam,
        downloadedFilterParam,
        batchCompleteFilterParam,
        processingFilterParam
    );

    const totalRecords = ingestionTrackingsRetrieved?.length || 0;

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

    const updateEmisLanding = emisLandingService.useModifyEmisLanding();
    const handleReDecrypt = (ingestionTracking: IngestionTracking) => {
        updateEmisLanding.updateIngestionTracking(ingestionTracking)
            .then(() => {
                toastSuccess("Ingestion Tracking Queued for Decrypt");
            })
            .catch(() => {
                toastError("error");
            });
    };

    const handleFilter = (
        supplier: SupplierView | null,
        decryptedFilter: boolean | undefined,
        downloadedFilter: boolean | undefined,
        batchCompleteFilter: boolean | undefined,
        processingFilter: boolean | undefined
    ) => {
        if (supplier === null) {
            setDebouncedSupplierTerm("");
            setSelectedSupplierId("");
        } else {
            setDebouncedSupplierTerm(supplier.id.toString());
            setSelectedSupplierId(supplier.id.toString());
        }
        setSelectedDecryptedFilter(decryptedFilter);
        setSelectedDownloadedFilter(downloadedFilter);
        setSelectedBatchCompleteFilter(batchCompleteFilter);
        setSelectedProcessingFilter(processingFilter);
    };

    const handleBatchClick = (batch: string) => {
        setSearchTerm(batch);
        setDebouncedTerm(batch);
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
        <div className="infiniteScrollContainer position-relative">
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        <FontAwesomeIcon icon={faDatabase} className="me-2" /> Ingestion Tracking
                    </CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll loading={isLoading || showSpinner} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
                            <Row className="m-0 p-0">
                                <div className="input-group mb-3 m-0 p-0">
                                    <SearchBase
                                        id="search"
                                        value={searchTerm}
                                        placeholder="Search for ingestion data..."
                                        onChange={(e) => {
                                            handleSearchChange(e.currentTarget.value);
                                        }}
                                    />
                                    <div className="btn-group" role="group" aria-label="Filter and Refresh buttons">
                                        <button
                                            className={`btn btn-outline-secondary ${isFilterActive ? "btn-outline-danger" : ""}`}
                                            id="filterButton"
                                            onClick={() => setShowSidePanel((prev) => !prev)}
                                        >
                                            <FontAwesomeIcon icon={faFilter} />
                                        </button>

                                        {showSpinner ? (
                                            <button className="btn btn-outline-secondary" disabled>
                                                <SpinnerBase />
                                            </button>
                                        ) : (
                                            <button
                                                className="btn btn-outline-secondary"
                                                id="refreshButton"
                                                onClick={refreshData}
                                            >
                                                <FontAwesomeIcon icon={faRefresh} />
                                            </button>
                                        )}
                                    </div>
                                </div>
                            </Row>
                            <div className="d-flex justify-content-between align-items-center mb-2">
                                <span>
                                </span>
                                <span className="text-muted small">
                                    Showing {totalRecords} record{totalRecords !== 1 ? "s" : ""}
                                </span>
                            </div>
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
                                                        onBatchClick={handleBatchClick}
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

            <div className={`filter-side-panel ${showSidePanel ? "open" : ""}`}>
                <div className="filter-side-panel-header">
                    <h5> <FontAwesomeIcon icon={faFilter} /> Filters</h5>
                    <button className="close-button" onClick={() => setShowSidePanel(false)}>×</button>
                </div>
                <div className="filter-side-panel-body">

                    <IngestionFilterModal
                        onAddFilter={handleFilter}
                        selectedSupplierId={selectedSupplierId}
                        initialDecryptedFilter={selectedDecryptedFilter}
                        initialDownloadedFilter={selectedDownloadedFilter}
                        initialBatchCompleteFilter={selectedBatchCompleteFilter}
                        initialProcessingFilter={selectedProcessingFilter}
                    />
                </div>
            </div>
        </div>
    );
};

export default IngestionTrackingTable;
