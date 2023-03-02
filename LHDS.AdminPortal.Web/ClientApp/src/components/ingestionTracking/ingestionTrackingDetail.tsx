import { Guid } from "guid-typescript";
import React, { FunctionComponent, useEffect, useState } from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import { documentViewService } from "../../services/views/Documents/DocumentViewService";
import { ingestionTrackingViewService } from "../../services/views/ingestionTrackingViewService";
import IngestionTrackingDetailCard from "./ingestionTrackingDetailCard";
import { toastSuccess } from "../../brokers/toastBroker";
import { documentService } from "../../services/foundations/documentService";

interface IngestionTrackingDetailProps {
    ingestionTrackingId: string;
    children?: React.ReactNode;
}

const IngestionTrackingDetail: FunctionComponent<IngestionTrackingDetailProps> = (props) => {
    const {
        ingestionTrackingId,
        children
    } = props;

    const { mappedIngestionTracking: ingestionTrackingRetrieved } =
        ingestionTrackingViewService.useGetIngestionTrackingById(Guid.parse(ingestionTrackingId))

    const [downloadFileName, setDownloadFileName] = useState<string>("");

    const { mappedLink } = documentService.useGetDownloadLinkByFileName(encodeURIComponent(downloadFileName))

    const handleDownload = async (ingestionTrackingView: IngestionTrackingView) => {
        if (ingestionTrackingView.decryptedFileName)
            setDownloadFileName(ingestionTrackingView.decryptedFileName)
        //const mappedLink = documentService.useGetDownloadLinkByFileName(ingestionTrackingView.fileName)
        //toastSuccess(`${mappedLink}`);
    }

    const handleReLand = async (ingestionTrackingView: IngestionTrackingView) => {
        toastSuccess("Re-Land");
    }

    const handleReDecrypt = async (ingestionTrackingView: IngestionTrackingView) => {
        toastSuccess("Re-Decrypt");
    }

    useEffect(() => {

    })

    return (
        <div>
            {ingestionTrackingRetrieved !== undefined && (
                <div>
                    <IngestionTrackingDetailCard
                        key={ingestionTrackingRetrieved.id.toString()}
                        ingestionTracking={ingestionTrackingRetrieved}
                        onDownload={handleDownload}
                        onReLand={handleReLand}
                        onReDecrypt={handleReDecrypt}
                    >

                        {children}
                    </IngestionTrackingDetailCard>
                </div>
            )}
        </div>
    );
}
export default IngestionTrackingDetail;