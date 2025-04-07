import React, { FunctionComponent} from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import { ingestionTrackingViewService } from "../../services/views/ingestionTrackingViewService";
import IngestionTrackingDetailCard from "./ingestionTrackingDetailCard";

interface IngestionTrackingDetailProps {
    ingestionTrackingId?: string;
    children?: React.ReactNode;
}

const IngestionTrackingDetail: FunctionComponent<IngestionTrackingDetailProps> = (props) => {
    const {
        ingestionTrackingId,
        children
    } = props;

    const { mappedIngestionTracking: ingestionTrackingRetrieved } =
        ingestionTrackingViewService.useGetIngestionTrackingById(ingestionTrackingId)

    const handleDownload = async (ingestionTrackingView: IngestionTrackingView) => {
        console.log(ingestionTrackingView);
    }

    const handleReDecrypt = async () => {
        console.log("TODO: Hanle ReDecrypt");
    };

    const handleRefresh = async () => {
        console.log("TODO: Refresh");
    }

    return (
        <div>
            {ingestionTrackingRetrieved !== undefined && (
                <div>
                    <IngestionTrackingDetailCard
                        key={ingestionTrackingRetrieved.id.toString()}
                        ingestionTracking={ingestionTrackingRetrieved}
                        onDownload={handleDownload}
                        onReDecrypt={handleReDecrypt}
                        onRefresh={handleRefresh}>

                        {children}
                    </IngestionTrackingDetailCard>
                </div>
            )}
        </div>
    );
}
export default IngestionTrackingDetail;