import { Guid } from "guid-typescript";
import React, { FunctionComponent, useEffect, useState } from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import { documentViewService } from "../../services/views/Documents/DocumentViewService";
import { ingestionTrackingViewService } from "../../services/views/ingestionTrackingViewService";
import IngestionTrackingDetailCard from "./ingestionTrackingDetailCard";

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

    const handleDownload = async (ingestionTrackingView: IngestionTrackingView) => {
        var pow = documentViewService.useGetDownloadLinkByFileName(ingestionTrackingView.fileName)
        alert(pow);
    }

    const handleReLand = async (ingestionTrackingView: IngestionTrackingView) => {
        alert("Re-Land");
    }

    const handleReDecrypt = async (ingestionTrackingView: IngestionTrackingView) => {
        alert("Re-Decrypt");
    }

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