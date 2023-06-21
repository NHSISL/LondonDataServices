import { Guid } from "guid-typescript";
import { Pds } from "../models/pds/pds";
import ApiBroker from "./apiBroker";

class PdsBroker {
    relativePdsUrl = '/api/Pds';

    private apiBroker: ApiBroker = new ApiBroker();

    async PostPdsAsync(pds: Pds) {
        return await this.apiBroker.PostAsync(this.relativePdsUrl, pds)
            .then(result => new Pds(result.data));
    }

    async GetAllPdsAsync(queryString: string) {
        const url = this.relativePdsUrl + queryString;
        if (queryString === "") {
            return undefined;
        }
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((optOut: any) => new Pds(optOut)));
    }

    async PutPdsAsync(pds: Pds) {
        return await this.apiBroker.PutAsync(this.relativePdsUrl, pds)
            .then(result => new Pds(result.data));
    }

    async DeletePdsByIdAsync(id: Guid) {
        const url = `${this.relativePdsUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new Pds(result.data));
    }
}

export default PdsBroker;