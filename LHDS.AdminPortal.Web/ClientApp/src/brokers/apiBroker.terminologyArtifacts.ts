import { Guid } from "guid-typescript";
import ApiBroker from "./apiBroker";
import { TerminologyArtifact } from "../models/terminologyArtifacts/terminologyArtifact";
import { AxiosResponse } from "axios";

class TerminologyArtifactBroker {
    relativeTerminologyArtifactUrl = '/api/TerminologyArtifacts';
    relativeTerminologyArtifactsOdataUrl = '/odata/TerminologyArtifacts'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((terminologyArtifact: any) => new TerminologyArtifact(terminologyArtifact));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostTerminologyArtifactAsync(terminologyArtifact: TerminologyArtifact) {
        return await this.apiBroker.PostAsync(this.relativeTerminologyArtifactUrl, terminologyArtifact)
            .then(result => new TerminologyArtifact(result.data));
    }

    async GetAllTerminologyArtifactsAsync(queryString: string) {
        let url = this.relativeTerminologyArtifactUrl + queryString;

        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((terminologyArtifact: any) => new TerminologyArtifact(terminologyArtifact)));
    }

    async GetTerminologyArtifactsFirstPagesAsync(query: string) {
        var url = this.relativeTerminologyArtifactsOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetTerminologyArtifactsSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetTerminologyArtifactByIdAsync(id: Guid) {
        const url = `${this.relativeTerminologyArtifactUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new TerminologyArtifact(result.data));
    }

    async PutTerminologyArtifactAsync(terminologyArtifact: TerminologyArtifact) {
        return await this.apiBroker.PutAsync(this.relativeTerminologyArtifactUrl, terminologyArtifact)
            .then(result => new TerminologyArtifact(result.data));
    }

    async DeleteTerminologyArtifactByIdAsync(id: Guid) {
        const url = `${this.relativeTerminologyArtifactUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new TerminologyArtifact(result.data));
    }
}

export default TerminologyArtifactBroker;