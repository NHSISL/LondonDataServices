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

type IngestionTrackingTableProps = {};

const IngestionTrackingTable: FunctionComponent<IngestionTrackingTableProps> = (props) => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");

    const {
        mappedIngestionTrackings: ingestionTrackingsRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
    } = ingestionTrackingHomeViewService.useGetAllIngestionTrackings(
        debouncedTerm
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

    const relandDocument = ingestionTrackingHomeViewService.useRelandIngestionTracking();
    const decryptDocument = ingestionTrackingHomeViewService.useRedecryptIngestionTracking();
    const downloadEncryptedDocument = ingestionTrackingHomeViewService.useDownloadEncryptedDocument();
    const downloadDecryptedDocument = ingestionTrackingHomeViewService.useDownloadDecryptedDocument();

    const handleRelanding = (ingestionTracking: IngestionTracking) => {
        return relandDocument.mutateAsync(ingestionTracking, {
            onSuccess: () => {
            },
            onError: (error: any) => {
            }
        });
    };

    const handleRedecrypt = (ingestionTracking: IngestionTracking) => {
        return decryptDocument.mutateAsync(ingestionTracking, {
            onSuccess: () => {
            },
            onError: (error: any) => {
            }
        });
    };

    const handleEncryptedDownload = (ingestionTracking: IngestionTracking) => {
        return downloadEncryptedDocument.mutateAsync(ingestionTracking, {
            onSuccess: () => {
            },
            onError: (error: any) => {
            }
        });
    };

    const handleDecryptedDownload = (ingestionTracking: IngestionTracking) => {
        return downloadDecryptedDocument.mutateAsync(ingestionTracking, {
            onSuccess: () => {
            },
            onError: (error: any) => {
            }
        });
    };

    const hasNoMorePages = () => {
        return !isLoading && data?.pages.at(-1)?.nextPage === undefined;
    };

    return (
        <div className="infiniteScrollContainer">
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>Ingestion Trackings</CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll loading={isLoading} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
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
                            </div>

                            <TableBase>
                                <TableBaseTbody>
                                    {ingestionTrackingsRetrieved?.map(
                                        (ingestionTrackingHomeView: IngestionTrackingHomeView) => (
                                            <IngestionTrackingRow
                                                key={ingestionTrackingHomeView.id}
                                                ingestionTracking={ingestionTrackingHomeView}
                                                onRelanding={handleRelanding}
                                                onRedecrypt={handleRedecrypt}
                                                onEncryptedDownload={handleEncryptedDownload}
                                                onDecryptedDownload={handleDecryptedDownload}
                                            />
                                        )
                                    )}
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