import { Guid } from "guid-typescript";
import React, { FunctionComponent} from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import { ingestionTrackingViewService } from "../../services/views/ingestionTrackingViewService";
import IngestionTrackingDetailCard from "./ingestionTrackingDetailCard";
import { toastSuccess } from "../../brokers/toastBroker";


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

    //const [downloadFileName, setDownloadFileName] = useState<string>("");
    //const { mappedLink } = documentService.useGetDownloadLinkByFileName(encodeURIComponent(downloadFileName))

    const handleDownload = async (ingestionTrackingView: IngestionTrackingView) => {
       // if (ingestionTrackingView.decryptedFileName)
        //    setDownloadFileName(ingestionTrackingView.decryptedFileName)
        //const mappedLink = documentService.useGetDownloadLinkByFileName(ingestionTrackingView.fileName)
        //toastSuccess(`${mappedLink}`);
    }

    const handleReLand = async (ingestionTrackingView: IngestionTrackingView) => {
        toastSuccess("Re-Land");
    }

    const handleReDecrypt = async (ingestionTrackingView: IngestionTrackingView) => {
        toastSuccess("Re-Decrypt");
    }

    const handleRefresh = async (ingestionTrackingView: IngestionTrackingView) => {
        toastSuccess("Refresh");
         //if (ingestionTrackingView.id)
         //   ingestionTrackingViewService.useGetIngestionTrackingById(Guid.parse(ingestionTrackingView.id))
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
                        onRefresh={handleRefresh}>

                        {children}
                    </IngestionTrackingDetailCard>
                </div>
            )}
        </div>
    );
}
export default IngestionTrackingDetail;