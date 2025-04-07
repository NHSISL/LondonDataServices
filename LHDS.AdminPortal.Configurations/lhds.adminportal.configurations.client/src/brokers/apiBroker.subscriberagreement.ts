import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";
import { SubscriberAgreement } from "../models/subscriberAgreements/subscriberAgreements";

class SubscriberAgreementBroker {
    relativeSubscriberAgreementUrl = '/api/subscriberAgreements';
    relativeSubscriberAgreementRegenerateUrl = '/api/subscriberAgreements/Regenerate';
    relativeSubscriberAgreementOdataUrl = '/odata/subscriberAgreements'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((subscriberAgreement: SubscriberAgreement) => new SubscriberAgreement(subscriberAgreement));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostSubscriberAgreementAsync(subscriberAgreement: SubscriberAgreement) {
        return await this.apiBroker.PostAsync(this.relativeSubscriberAgreementUrl, subscriberAgreement)
            .then(result => new SubscriberAgreement(result.data));
    }

    async GetAllSubscriberAgreementsAsync(queryString: string) {
        const url = this.relativeSubscriberAgreementUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((subscriberAgreement: SubscriberAgreement) => new SubscriberAgreement(subscriberAgreement)));
    }

    async GetSubscriberAgreementFirstPagesAsync(query: string) {
        const url = this.relativeSubscriberAgreementOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetSubscriberAgreementSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetSubscriberAgreementByIdAsync(id: string) {
        const url = `${this.relativeSubscriberAgreementUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new SubscriberAgreement(result.data));
    }

    async PutSubscriberAgreementAsync(subscriberAgreement: SubscriberAgreement) {
        return await this.apiBroker.PutAsync(this.relativeSubscriberAgreementUrl, subscriberAgreement)
            .then(result => new SubscriberAgreement(result.data));
    }

    async PutRegenerateSubscriberAgreementAsync(subscriberAgreement: SubscriberAgreement) {
        return await this.apiBroker.PutAsync(this.relativeSubscriberAgreementRegenerateUrl, subscriberAgreement)
            .then(result => new SubscriberAgreement(result.data));
    }

    async DeleteSubscriberAgreementByIdAsync(id: string) {
        const url = `${this.relativeSubscriberAgreementUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new SubscriberAgreement(result.data));
    }
}

export default SubscriberAgreementBroker;