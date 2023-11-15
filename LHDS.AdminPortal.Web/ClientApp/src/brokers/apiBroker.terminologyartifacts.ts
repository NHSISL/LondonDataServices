import { Guid } from "guid-typescript";
import ApiBroker from "./apiBroker";
import { TerminologyArtifact } from "../models/terminologyArtifacts/terminologyArtifact";

class TerminologyArtifactBroker {
    relativeTerminologyArtifactUrl = '/api/TerminologyArtifacts';

    private apiBroker: ApiBroker = new ApiBroker();

    async PostTerminologyArtifactAsync(terminologyArtifact: TerminologyArtifact) {
        return await this.apiBroker.PostAsync(this.relativeTerminologyArtifactUrl, terminologyArtifact)
            .then(result => new TerminologyArtifact(result.data));
    }

    async GetAllTerminologyArtifactsAsync(queryString: string) {
        let url = this.relativeTerminologyArtifactUrl + queryString;

        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((terminologyArtifact: any) => new TerminologyArtifact(terminologyArtifact)));
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