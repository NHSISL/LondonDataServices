import { useQueryClient, useMutation, } from "@tanstack/react-query";
import EmisLandingBroker from "../../brokers/apiBroker.emislandings";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import { useMsal } from "@azure/msal-react";

export const emisLandingService = {

    useModifyEmisLanding: () => {
        const broker = new EmisLandingBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (ingestionTracking: IngestionTracking) => {
                const date = new Date();
                ingestionTracking.updatedDate = date;
                ingestionTracking.updatedBy = msal.accounts[0].username;

                return broker.PutRedecryptDocumentByIngestionTrackingIdAsync(ingestionTracking);
            },

            onSuccess: () => {
                queryClient.invalidateQueries({ queryKey: ["IngestionTrackingGetAll"] });
            }
        });
    },
}