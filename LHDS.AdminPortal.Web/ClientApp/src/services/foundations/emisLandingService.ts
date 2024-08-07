import { useMsal } from "@azure/msal-react";
import { useMutation, useQueryClient } from "react-query";
import EmisLandingBroker from "../../brokers/apiBroker.emislandings";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";

export const emisLandingService = {
    useModifyEmisLanding: () => {
        const broker = new EmisLandingBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((ingestionTracking: IngestionTracking) => {
            const date = new Date();
            ingestionTracking.updatedDate = date;
            ingestionTracking.updatedBy = msal.accounts[0].username;

            return broker.PutRedecryptDocumentByIngestionTrackingIdgAsync(ingestionTracking);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("IngestionTrackingGetAll");
                }
            });
    }
}