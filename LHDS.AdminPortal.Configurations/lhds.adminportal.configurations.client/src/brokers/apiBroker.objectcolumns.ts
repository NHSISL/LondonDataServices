import { ObjectColumn } from "../models/objectColumns/objectColumn";
import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";

class ObjectColumnBroker {
    relativeObjectColumnUrl = '/api/objectColumns';
    relativeObjectColumnOdataUrl = '/odata/objectColumns'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((objectColumn: ObjectColumn) => new ObjectColumn(objectColumn));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostObjectColumnAsync(objectColumn: ObjectColumn) {
        return await this.apiBroker.PostAsync(this.relativeObjectColumnUrl, objectColumn)
            .then(result => new ObjectColumn(result.data));
    }

    async GetAllObjectColumnsAsync(queryString: string) {
        const url = this.relativeObjectColumnUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((objectColumn: ObjectColumn) => new ObjectColumn(objectColumn)));
    }

    async GetObjectColumnFirstPagesAsync(query: string) {
        const url = this.relativeObjectColumnOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetObjectColumnSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetObjectColumnByIdAsync(id: string) {
        const url = `${this.relativeObjectColumnUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new ObjectColumn(result.data));
    }

    async PutObjectColumnAsync(objectColumn: ObjectColumn) {
        return await this.apiBroker.PutAsync(this.relativeObjectColumnUrl, objectColumn)
            .then(result => new ObjectColumn(result.data));
    }

    async DeleteObjectColumnByIdAsync(id: string) {
        const url = `${this.relativeObjectColumnUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new ObjectColumn(result.data));
    }
}

export default ObjectColumnBroker;