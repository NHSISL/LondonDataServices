import { useEffect, useState } from "react";
import { Audit } from "../../models/audits/audit";
import { AuditView } from "../../models/views/components/audit/auditView";
import { auditService } from "../foundations/auditService";

export const auditViewService = {

    useGetAllAudits: (ingestionTrackingId: string) => {
        try {

            let query = `?$orderby=createdDate`;

            if (ingestionTrackingId) {
                query = query + `&$filter=ingestionTrackingId eq ${ingestionTrackingId}`;
            }

            const response = auditService.useGetAllAudits(query);
            const [mappedAudits, setMappedAudits] = useState<Array<AuditView>>([]);

            useEffect(() => {
                if (response.data) {
                    const audits = response.data.map((audit: Audit) =>
                        new AuditView(
                            audit.id,
                            audit.ingestionTrackingId,
                            audit.message,
                            audit.createdDate,
                        ));

                    setMappedAudits(audits);
                }
            }, [response.data]);

            return {
                mappedAudits,
                ...response,
            };
        } catch (err) {
            throw err;
        }
    }
}