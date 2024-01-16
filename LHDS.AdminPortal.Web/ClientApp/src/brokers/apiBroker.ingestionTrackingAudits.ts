import { IngestionTrackingAudit } from "../models/ingestionTrackingAudits/ingestionTrackingAudits";
import ApiBroker from "./apiBroker";

class IngestionTrackingAuditBroker {
    relativeAuditUrl = '/api/ingestionTrackingAudits';

    private apiBroker: ApiBroker = new ApiBroker();

    async GetAllIngestionTrackingAuditsAsync(queryString: string) {
        const url = this.relativeAuditUrl + queryString;

        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((ingestionTrackingAudit: any) => new IngestionTrackingAudit(ingestionTrackingAudit)));
    }
}

export default IngestionTrackingAuditBroker;