import { useQuery } from "react-query";
import DocumentBroker from "../../brokers/apiBroker.documents";

export const documentService = {
    useGetDownloadLinkByFileName: (fileName: string) => {
        const documentBroker = new DocumentBroker();

        return useQuery(
            ["DownloadLinkByFileName", { fileName: fileName }],
            () => documentBroker.GetDownloadLinkAsync(fileName),
            { staleTime: Infinity });
    }
}