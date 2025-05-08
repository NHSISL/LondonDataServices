import { Guid } from "guid-typescript";
import React, { FunctionComponent} from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import { ingestionTrackingViewService } from "../../services/views/ingestionTrackingViewService";
import IngestionTrackingDetailCard from "./ingestionTrackingDetailCard";
import { toastError, toastSuccess } from "../../brokers/toastBroker";
import { emisLandingService } from "../../services/foundations/emisLandingService";

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

    const updateEmisLanding = emisLandingService.useModifyEmisLanding();

    const handleReDecrypt = async (ingestionTrackingView: IngestionTrackingView) => {
        updateEmisLanding.updateIngestionTracking(ingestionTrackingView)
            .then(() => {
                toastSuccess("Ingestion Tracking Queued for Decrypt")
            })
            .catch(e => {
            toastError("error")
        });
    };

    const handleRefresh = async (ingestionTrackingView: IngestionTrackingView) => {}

    return (
        <>
            {ingestionTrackingRetrieved !== undefined && (
                    <IngestionTrackingDetailCard
                        key={ingestionTrackingRetrieved.id.toString()}
                        ingestionTracking={ingestionTrackingRetrieved}
                        onReDecrypt={handleReDecrypt}
                        onRefresh={handleRefresh}>

                        {children}
                    </IngestionTrackingDetailCard>
            )}
        </>
    );
}
export default IngestionTrackingDetail;