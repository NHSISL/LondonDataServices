import { useQuery } from "react-query";
import DecryptionBroker from "../../brokers/apiBroker.decryptions";

export const decryptionService = {
    useGetDownloadLinkByFileName: (fileName: string) => {
        const decryptionBroker = new DecryptionBroker();

        return useQuery(
            ["DecryptionGetDocumentByFileNameAsync", { fileName: fileName }],
            () => decryptionBroker.GetDocumentByFileNameToDecryptAsync(fileName),
            { staleTime: Infinity });
    }
}