import { useMutation } from "@tanstack/react-query";
import LandingBroker from "../../brokers/apiBroker.landings";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";

export const landingService = {

    useGetDownloadLinkByFileName: () => {
        const landingBroker = new LandingBroker();

        return useMutation({
            mutationFn: (ingestionTracking: IngestionTracking) => {
                return landingBroker.GetLandingDocumentByFileNameAsync(ingestionTracking.fileName);
            },
            onSuccess: (data) => {
                console.log("Download link retrieved successfully:", data);
            },
            onError: (error) => {
                console.error("Error retrieving download link:", error);
            }
        }
        );
    }
};