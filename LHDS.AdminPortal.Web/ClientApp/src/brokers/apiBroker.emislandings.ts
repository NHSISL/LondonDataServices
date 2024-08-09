import { IngestionTracking } from "../models/ingestionTrackings/ingestionTracking";
import ApiBroker from "./apiBroker";

class EmisLandingBroker {
    relativeEmisLandingUrl = '/api/emisLandings';

    private apiBroker: ApiBroker = new ApiBroker();

    async PutRedecryptDocumentByIngestionTrackingIdAsync(ingestionTracking: IngestionTracking) {
        var ingestionTrackingId = ingestionTracking.id;
        const url = `${this.relativeEmisLandingUrl}/decrypt/${ingestionTrackingId}`;

        return await this.apiBroker.PutAsync(url, {})
            .then(result => new IngestionTracking(result.data));
    }
}
export default EmisLandingBroker;