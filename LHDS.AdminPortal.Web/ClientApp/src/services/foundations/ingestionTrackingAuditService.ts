import { useQuery } from "react-query";
import IngestionTrackingAuditBroker from "../../brokers/apiBroker.ingestionTrackingAudits";

export const ingestionTrackingAuditService = {
    useGetAllIngestionTrackingAudits: (query: string) => {
        const ingestionTrackingAuditBroker = new IngestionTrackingAuditBroker();

        return useQuery(
            ["IngestionTrackingAuditGetAll", { query: query }],
            () => ingestionTrackingAuditBroker.GetAllIngestionTrackingAuditsAsync(query),
            { staleTime: Infinity });
    }
}