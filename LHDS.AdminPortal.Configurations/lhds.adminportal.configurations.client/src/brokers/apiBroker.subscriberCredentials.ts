import { Guid } from "guid-typescript";
import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";
import { SubscriberCredential } from "../models/subscriberCredentials/subscriberCredentials";

class SubscriberCredentialBroker {
    relativeSubscriberCredentialUrl = '/api/subscriberCredentials';
    relativeSubscriberCredentialRegenerateUrl = '/api/subscriberCredentials/regeneratekeys';
    relativeSubscriberCredentialOdataUrl = '/odata/subscriberCredentials'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((subscriberCredential: any) => new SubscriberCredential(subscriberCredential));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostSubscriberCredentialAsync(subscriberCredential: SubscriberCredential) {
        return await this.apiBroker.PostAsync(this.relativeSubscriberCredentialUrl, subscriberCredential)
            .then(result => new SubscriberCredential(result.data));
    }

    async PostSubscriberCredentialAndRegenerateKeysAsync(subscriberCredential: SubscriberCredential) {
        return await this.apiBroker.PostAsync(this.relativeSubscriberCredentialRegenerateUrl, subscriberCredential)
            .then(result => new SubscriberCredential(result.data));
    }
    
    async GetAllSubscriberCredentialsAsync(queryString: string) {
        const url = this.relativeSubscriberCredentialUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((subscriberCredential: any) => new SubscriberCredential(subscriberCredential)));
    }

    async GetSubscriberCredentialFirstPagesAsync(query: string) {
        var url = this.relativeSubscriberCredentialOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetSubscriberCredentialSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetSubscriberCredentialByIdAsync(id: Guid) {
        const url = `${this.relativeSubscriberCredentialUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new SubscriberCredential(result.data));
    }

    async PutSubscriberCredentialAsync(subscriberCredential: SubscriberCredential) {
        return await this.apiBroker.PutAsync(this.relativeSubscriberCredentialUrl, subscriberCredential)
            .then(result => new SubscriberCredential(result.data));
    }

    async PutRegenerateSubscriberCredentialAsync(subscriberCredential: SubscriberCredential) {
        return await this.apiBroker.PutAsync(this.relativeSubscriberCredentialRegenerateUrl, subscriberCredential)
            .then(result => new SubscriberCredential(result.data));
    }

    async DeleteSubscriberCredentialByIdAsync(id: Guid) {
        const url = `${this.relativeSubscriberCredentialUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new SubscriberCredential(result.data));
    }
}

export default SubscriberCredentialBroker;