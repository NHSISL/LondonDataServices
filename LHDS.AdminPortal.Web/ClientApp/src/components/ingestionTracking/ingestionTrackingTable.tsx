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
    const [showSpinner, setShowSpinner] = useState(false);
    const [showModal, setShowModal] = useState(false);
    

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

    const downloadEncryptedDocument = ingestionTrackingHomeViewService.useDownloadEncryptedDocument();
    const handleEncryptedDownload = (ingestionTracking: IngestionTracking) => {
        return downloadEncryptedDocument.mutateAsync(ingestionTracking, {
            onSuccess: () => {
            },
            onError: (error: any) => {
            }
        });
    };

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
                            <Row>
                                <div className="input-group mb-3">
                                    <SearchBase
                                        id="search"
                                        value={searchTerm}
                                        placeholder="Search for ingestion data..."
                                        onChange={(e) => {handleSearchChange(e.currentTarget.value)}} />

                                    <div className="input-group-append">
                                        <button
                                            className="btn btn-outline-secondary"
                                            id="filterButton"
                                            onClick={() => setShowModal(true)}>

                                            <FontAwesomeIcon icon={faFilter}/> Filter
                                        </button>
                                    </div>

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
                                                        onEncryptedDownload={handleEncryptedDownload}
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