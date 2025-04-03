import { useQuery } from "@tanstack/react-query";
import IngestionTrackingAuditBroker from "../../brokers/apiBroker.ingestionTrackingAudits";

export const ingestionTrackingAuditService = {

    useGetAllIngestionTrackingAudits: (query: string) => {
        const ingestionTrackingAuditBroker = new IngestionTrackingAuditBroker();

        return useQuery({
            queryKey: ["IngestionTrackingAuditGetAll", { query: query }],
            queryFn: () => ingestionTrackingAuditBroker.GetAllIngestionTrackingAuditsAsync(query),
            staleTime: Infinity
        });
    }
}