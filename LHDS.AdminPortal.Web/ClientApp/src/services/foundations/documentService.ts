import { useMutation } from "react-query";
import DocumentBroker from "../../brokers/apiBroker.documents";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";

export const documentService = {
    useGetDownloadLinkByFileName: () => {
        const documentBroker = new DocumentBroker();

        return useMutation((ingestionTracking: IngestionTracking) => {
            return documentBroker.GetDownloadLinkAsync(ingestionTracking.fileName);
        },
            {
                onSuccess: (variables) => {
                }
            });
    }
}