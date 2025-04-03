import { AxiosResponse } from "axios";
import { Guid } from "guid-typescript";
import { IngestionTracking } from "../models/ingestionTrackings/ingestionTracking";
import ApiBroker from "./apiBroker";

class IngestionTrackingBroker {
    relativeIngestionTrackingUrl = '/api/ingestionTrackings';
    relativeIngestionTrackingOdataUrl = '/odata/ingestionTrackings'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((ingestionTracking: IngestionTracking) => new IngestionTracking(ingestionTracking));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async GetAllIngestionTrackingsAsync(queryString: string) {
        const url = this.relativeIngestionTrackingUrl + queryString;

        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((ingestionTracking: IngestionTracking) => new IngestionTracking(ingestionTracking)));
    }

    async GetIngestionTrackingFirstPagesAsync(query: string) {
        const url = this.relativeIngestionTrackingOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetIngestionTrackingsSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetIngestionTrackingByIdAsync(id: Guid) {
        const url = `${this.relativeIngestionTrackingUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new IngestionTracking(result.data));
    }

    async PutIngestionTrackingAsync(ingestionTracking: IngestionTracking) {
        return await this.apiBroker.PutAsync(this.relativeIngestionTrackingUrl, ingestionTracking)
            .then(result => new IngestionTracking(result.data));
    }
}
export default IngestionTrackingBroker;