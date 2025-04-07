import { SpecificationObject } from "../models/specificationObjects/specificationObject";
import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";

class SpecificationObjectBroker {
    relativeSpecificationObjectUrl = '/api/specificationObjects';
    relativeSpecificationObjectOdataUrl = '/odata/specificationObjects'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((specificationObject: SpecificationObject) => new SpecificationObject(specificationObject));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostSpecificationObjectAsync(specificationObject: SpecificationObject) {
        return await this.apiBroker.PostAsync(this.relativeSpecificationObjectUrl, specificationObject)
            .then(result => new SpecificationObject(result.data));
    }

    async GetAllSpecificationObjectsAsync(queryString: string) {
        const url = this.relativeSpecificationObjectUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((specificationObject: SpecificationObject) => new SpecificationObject(specificationObject)));
    }

    async GetSpecificationObjectFirstPagesAsync(query: string) {
        const url = this.relativeSpecificationObjectOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetSpecificationObjectSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetSpecificationObjectByIdAsync(id: string) {
        const url = `${this.relativeSpecificationObjectUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new SpecificationObject(result.data));
    }

    async PutSpecificationObjectAsync(specificationObject: SpecificationObject) {
        return await this.apiBroker.PutAsync(this.relativeSpecificationObjectUrl, specificationObject)
            .then(result => new SpecificationObject(result.data));
    }

    async DeleteSpecificationObjectByIdAsync(id: string) {
        const url = `${this.relativeSpecificationObjectUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new SpecificationObject(result.data));
    }
}

export default SpecificationObjectBroker;