import { Audit } from "../models/audits/audit";
import ApiBroker from "./apiBroker";

class AuditBroker {
    relativeAuditUrl = '/api/ingestionTrackingAudits';

    private apiBroker: ApiBroker = new ApiBroker();

    async GetAllAuditsAsync(queryString: string) {
        const url = this.relativeAuditUrl + queryString;

        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((audit: any) => new Audit(audit)));
    }
}
export default AuditBroker;