import { useQuery } from "react-query";
import AuditBroker from "../../brokers/apiBroker.audits";

export const auditService = {
    useGetAllAudits: (query: string) => {
        const auditBroker = new AuditBroker();

        return useQuery(
            ["AuditGetAll", { query: query }],
            () => auditBroker.GetAllAuditsAsync(query),
            { staleTime: Infinity });
    }
}