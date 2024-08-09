import { useQueryClient } from "react-query";
import EmisLandingBroker from "../../brokers/apiBroker.emislandings";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";

export const emisLandingService = {
    useModifyEmisLanding: () => {
        const broker = new EmisLandingBroker();
        const queryClient = useQueryClient();

        return {
            updateIngestionTracking: (ingestionTracking: IngestionTracking) => {
                return broker.PutRedecryptDocumentByIngestionTrackingIdAsync(ingestionTracking)
                    .then(() => {
                        queryClient.invalidateQueries("IngestionTrackingGetAll");
                    });
            }
        }
    }
}