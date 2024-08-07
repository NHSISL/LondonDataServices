import { Guid } from "guid-typescript";
import { IngestionTracking } from "../models/ingestionTrackings/ingestionTracking";
import ApiBroker from "./apiBroker";

class EmisLandingBroker {
    relativeEmisLandinggUrl = '/api/emisLanding';

    private apiBroker: ApiBroker = new ApiBroker();

    async PutRedecryptDocumentByIngestionTrackingIdgAsync(ingestionTrackingId: Guid) {
        return await this.apiBroker.PutAsync(this.relativeEmisLandinggUrl, ingestionTrackingId)
            .then(result => new IngestionTracking(result.data));
    }
}
export default EmisLandingBroker;