import { Guid } from "guid-typescript";
import { DataSetObject } from "../models/dataSetObjects/dataSetObject";
import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";

class DataSetObjectBroker {
    relativeDataSetObjectUrl = '/api/dataSetObjects';
    relativeDataSetObjectOdataUrl = '/odata/dataSetObjects'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((dataSetObject: any) => new DataSetObject(dataSetObject));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostDataSetObjectAsync(dataSetObject: DataSetObject) {
        return await this.apiBroker.PostAsync(this.relativeDataSetObjectUrl, dataSetObject)
            .then(result => new DataSetObject(result.data));
    }

    async GetAllDataSetObjectsAsync(queryString: string) {
        const url = this.relativeDataSetObjectUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((optOut: any) => new DataSetObject(optOut)));
    }

    async GetDataSetObjectFirstPagesAsync(query: string) {
        var url = this.relativeDataSetObjectOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetDataSetObjectSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetDataSetObjectByIdAsync(id: Guid) {
        const url = `${this.relativeDataSetObjectUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new DataSetObject(result.data));
    }
}

export default DataSetObjectBroker;