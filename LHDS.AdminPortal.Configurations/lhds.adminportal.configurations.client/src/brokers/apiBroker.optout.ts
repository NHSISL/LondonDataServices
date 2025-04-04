import { OptOut } from "../models/optout/optout";
import ApiBroker from "./apiBroker";

class OptOutBroker {
    relativeOptOutUrl = '/api/OptOuts';

    private apiBroker: ApiBroker = new ApiBroker();

    async PostOptOutAsync(optout: OptOut) {
        return await this.apiBroker.PostAsync(this.relativeOptOutUrl, optout)
            .then(result => new OptOut(result.data));
    }

    async GetAllOptOutsAsync(queryString: string) {
        const url = this.relativeOptOutUrl + queryString;
        if (queryString === "/") {
            return undefined;
        }
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((optOut: OptOut) => new OptOut(optOut)));
    }

    async GetOptOutByNhsNumberAsync(nhsnumber: string) {
        if (!nhsnumber) {
            return undefined;
        }

        const url = `${this.relativeOptOutUrl}/${nhsnumber}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new OptOut(result.data));
    }

    async PutOptOutAsync(optout: OptOut) {
        return await this.apiBroker.PutAsync(this.relativeOptOutUrl, optout)
            .then(result => new OptOut(result.data));
    }

    async DeleteOptOutByIdAsync(id: string) {
        const url = `${this.relativeOptOutUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new OptOut(result.data));
    }
}

export default OptOutBroker;