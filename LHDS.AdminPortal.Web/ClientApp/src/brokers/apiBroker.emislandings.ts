import { IngestionTracking } from "../models/ingestionTrackings/ingestionTracking";
import ApiBroker from "./apiBroker";

class EmisLandingBroker {
    relativeEmisLandinggUrl = '/api/emisLanding';

    private apiBroker: ApiBroker = new ApiBroker();

    async PutRedecryptDocumentByIngestionTrackingIdgAsync(ingestionTracking: IngestionTracking) {
        return await this.apiBroker.PutAsync(this.relativeEmisLandinggUrl, ingestionTracking.Id)
            .then(result => new IngestionTracking(result.data));
    }
}
export default EmisLandingBroker;