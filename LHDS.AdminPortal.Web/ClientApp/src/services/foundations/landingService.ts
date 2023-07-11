import { useQuery } from "react-query";
import LandingBroker from "../../brokers/apiBroker.landings";

export const landingService = {
    useGetDownloadLinkByFileName: (fileName: string) => {
        const landingBroker = new LandingBroker();

        return useQuery(
            ["LandingGetDocumentByFileNameAsync", { fileName: fileName }],
            () => landingBroker.GetLandingDocumentByFileNameAsync(fileName),
            { staleTime: Infinity });
    }
}