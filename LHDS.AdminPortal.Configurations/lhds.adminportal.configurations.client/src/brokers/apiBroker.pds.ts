import { Pds } from "../models/pds/pds";
import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";

class PdsBroker {
    relativePdsUrl = '/api/pdsAudits';
    relativePdsOdataUrl = '/odata/pdsAudits'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((pds: Pds) => new Pds(pds));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async GetAllPdsAsync(queryString: string) {
        const url = this.relativePdsUrl + queryString;
        if (queryString === "") {
            return undefined;
        }
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((pds: Pds) => new Pds(pds)));
    }

    async GetPdsFirstPagesAsync(query: string) {
        const url = this.relativePdsOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetPdsSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async PostPdsAsync(pds: Pds) {
        return await this.apiBroker.PostAsync(this.relativePdsUrl, pds)
            .then(result => new Pds(result.data));
    }

    async PutPdsAsync(pds: Pds) {
        return await this.apiBroker.PutAsync(this.relativePdsUrl, pds)
            .then(result => new Pds(result.data));
    }

    async DeletePdsByIdAsync(id: string) {
        const url = `${this.relativePdsUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new Pds(result.data));
    }
}

export default PdsBroker;