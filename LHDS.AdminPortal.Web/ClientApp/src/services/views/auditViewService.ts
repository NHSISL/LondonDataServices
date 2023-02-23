import { Guid } from "guid-typescript";
import { useEffect, useState } from "react";
import { Audit } from "../../models/audits/audit";
import { AuditView } from "../../models/views/components/audit/auditView";
import { auditService } from "../foundations/auditService";

export const auditViewService = {

    useGetAllAudits: (searchTerm?: string) => {
        try {
            let query = '?$orderby=createdDate';

            if (searchTerm) {
                query = query + `&$filter=contains(Message,'${searchTerm}')`;
            }

            const response = auditService.useGetAllAudits(query);
            const [mappedAudits, setMappedAudits] = useState<Array<AuditView>>([]);

            useEffect(() => {
                if (response.data) {
                    const audits = response.data.map((audit: Audit) =>
                        new AuditView(
                            audits.id,
                            audits.ingestionTrackingId,
                            audits.message,
                            audits.createdDate,
                        ));

                    setMappedAudits(audits);
                }
            }, [response.data]);

            return {
                mappedAudits, ...response
            }
        } catch (err) {
            throw err;
        }
    }
}