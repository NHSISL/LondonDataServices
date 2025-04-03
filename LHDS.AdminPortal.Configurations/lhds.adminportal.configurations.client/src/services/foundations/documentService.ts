import { useMutation } from "@tanstack/react-query";
import DocumentBroker from "../../brokers/apiBroker.documents";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";

export const documentService = {
    useGetDownloadLinkByFileName: () => {
        const documentBroker = new DocumentBroker();

        return useMutation({
            mutationFn: (ingestionTracking: IngestionTracking) => {
                return documentBroker.GetDownloadLinkAsync(ingestionTracking.fileName);
            },
            onSuccess: (data, variables: IngestionTracking) => {
                console.log(`Download link retrieved for file: ${variables.fileName}`);
                // Handle success logic here
            },
            onError: (error, variables: IngestionTracking) => {
                console.error(`Failed to retrieve download link for file: ${variables.fileName}`, error);
                // Handle error logic here
            }
        });
    }
};