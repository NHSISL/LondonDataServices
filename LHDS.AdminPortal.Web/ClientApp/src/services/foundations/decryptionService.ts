import { useMutation } from "react-query";
import DecryptionBroker from "../../brokers/apiBroker.decryptions";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";

export const decryptionService = {
    useGetDocumentByFileNameToDecryptAsync: () => {
        const decryptionBroker = new DecryptionBroker();

        return useMutation((ingestionTracking: IngestionTracking) => {
            return decryptionBroker.GetDocumentByFileNameToDecryptAsync(ingestionTracking.fileName);
        },
            {
                onSuccess: (variables) => {
                }
            });
    }
}