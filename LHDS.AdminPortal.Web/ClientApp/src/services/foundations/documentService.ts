import { useEffect, useState } from "react";
import { useQuery } from "react-query";
import DocumentBroker from "../../brokers/apiBroker.documents";
import { Document } from "../../models/documents/document";

export const documentService = {
    useGetDownloadLinkByFileName: (fileName: string) => {
        const documentBroker = new DocumentBroker();
        const [mappedLink, setMappedLink] = useState<Document>();

        const query = useQuery(
            ["DownloadLinkByFileName", { fileName: fileName }],
            () => documentBroker.GetDownloadLinkAsync(fileName),
            { enabled: !!fileName ,  staleTime: Infinity });

        useEffect(() => {
            setMappedLink(query.data)
        },[query.data])

        return {...query,mappedLink}
    }

    
}