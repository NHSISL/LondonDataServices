import { useMutation } from "@tanstack/react-query";
import DecryptionBroker from "../../brokers/apiBroker.decryptions";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";

export const decryptionService = {
    useGetDocumentByFileNameToDecryptAsync: () => {
        const decryptionBroker = new DecryptionBroker();

        return useMutation({
            mutationFn: (ingestionTracking: IngestionTracking) => {
                return decryptionBroker.GetDocumentByFileNameToDecryptAsync(ingestionTracking.fileName);
            },
            onSuccess: (data, variables: IngestionTracking) => {
                console.log(`Decryption successful for file: ${variables.fileName}`);
            },
            onError: (error, variables: IngestionTracking) => {
                console.error(`Decryption failed for file: ${variables.fileName}`, error);
            }
        });
    }
};