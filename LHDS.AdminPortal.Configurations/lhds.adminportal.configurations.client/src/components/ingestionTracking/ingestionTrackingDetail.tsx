import React, { FunctionComponent} from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import { ingestionTrackingViewService } from "../../services/views/ingestionTrackingViewService";
import IngestionTrackingDetailCard from "./ingestionTrackingDetailCard";
import { toastSuccess } from "../../brokers/toastBroker.success";
import { toastError } from "../../brokers/toastBroker.error";
import { emisLandingService } from "../../services/foundations/emisLandingService";

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


    //const [downloadFileName, setDownloadFileName] = useState<string>("");
    //const { mappedLink } = documentService.useGetDownloadLinkByFileName(encodeURIComponent(downloadFileName))

    const handleDownload = async (ingestionTrackingView: IngestionTrackingView) => {
       // if (ingestionTrackingView.decryptedFileName)
        //    setDownloadFileName(ingestionTrackingView.decryptedFileName)
        //const mappedLink = documentService.useGetDownloadLinkByFileName(ingestionTrackingView.fileName)
        //toastSuccess(`${mappedLink}`);
    }

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