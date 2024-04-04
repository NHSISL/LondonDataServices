import { useEffect, useState } from "react";
import { IngestionTrackingAudit } from "../../../models/ingestionTrackingAudits/ingestionTrackingAudits";
import { IngestionTrackingAuditView } from "../../../models/views/components/ingestionTrackingAudit/ingestionTrackingAuditView";
import { ingestionTrackingAuditService } from "../../foundations/ingestionTrackingAuditService";

export const ingestionTrackingAuditViewService = {

    useGetAllAudits: (ingestionTrackingId: string) => {
        try {

            let query = `?$orderby=CreatedDate`;

            if (ingestionTrackingId) {
                query = query + `&$filter=ingestionTrackingId eq ${ingestionTrackingId}`;
            }

            const response = ingestionTrackingAuditService.useGetAllIngestionTrackingAudits(query);
            const [mappedAudits, setMappedAudits] = useState<Array<IngestionTrackingAuditView>>([]);

            useEffect(() => {
                if (response.data) {
                    const ingestionTrackingAudits = response.data.map((ingestionTrackingAudit: IngestionTrackingAudit) =>
                        new IngestionTrackingAuditView(
                            ingestionTrackingAudit.id,
                            ingestionTrackingAudit.ingestionTrackingId,
                            ingestionTrackingAudit.message,
                            ingestionTrackingAudit.createdDate,
                        ));

                    setMappedAudits(ingestionTrackingAudits);
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