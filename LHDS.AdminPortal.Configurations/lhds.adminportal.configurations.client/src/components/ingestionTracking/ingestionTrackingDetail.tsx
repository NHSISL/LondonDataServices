import React, { FunctionComponent} from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import { ingestionTrackingViewService } from "../../services/views/ingestionTrackingViewService";
import IngestionTrackingDetailCard from "./ingestionTrackingDetailCard";
import { emisLandingService } from "../../services/foundations/emisLandingService";
import { toastSuccess } from "../../brokers/toastBroker.success";
import { toastError } from "../../brokers/toastBroker.error";

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
        ingestionTrackingViewService.useGetIngestionTrackingById(ingestionTrackingId);

    const updateEmisLanding = emisLandingService.useModifyEmisLanding();

    const handleReDecrypt = async (ingestionTrackingView: IngestionTrackingView) => {
        updateEmisLanding.updateIngestionTracking(ingestionTrackingView)
            .then(() => {
                toastSuccess("Ingestion Tracking Queued for Decrypt")
            })
            .catch(e => {
                const message = e?.message || JSON.stringify(e);
                toastError(`Error queuing ingestion tracking: ${message}`);
        });
    };

    return (
        <>
            {ingestionTrackingRetrieved !== undefined && (
                    <IngestionTrackingDetailCard
                        key={ingestionTrackingRetrieved.id.toString()}
                        ingestionTracking={ingestionTrackingRetrieved}
                        onReDecrypt={handleReDecrypt}>

                        {children}
                    </IngestionTrackingDetailCard>
            )}
        </>
    );
}
export default IngestionTrackingDetail;