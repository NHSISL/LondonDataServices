import { DataSet } from "../models/dataSets/dataSet";
import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";

class DataSetBroker {
    relativeDataSetUrl = '/api/dataSets';
    relativeDataSetOdataUrl = '/odata/dataSets'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((dataSet: DataSet) => new DataSet(dataSet));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostDataSetAsync(dataSet: DataSet) {
        return await this.apiBroker.PostAsync(this.relativeDataSetUrl, dataSet)
            .then(result => new DataSet(result.data));
    }

    async GetAllDataSetsAsync(queryString: string) {
        const url = this.relativeDataSetUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((dataSet: DataSet) => new DataSet(dataSet)));
    }

    async GetDataSetFirstPagesAsync(query: string) {
        const url = this.relativeDataSetOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetDataSetSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetDataSetByIdAsync(id: string) {
        const url = `${this.relativeDataSetUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new DataSet(result.data));
    }

    async PutDataSetAsync(dataSet: DataSet) {
        return await this.apiBroker.PutAsync(this.relativeDataSetUrl, dataSet)
            .then(result => new DataSet(result.data));
    }

    async DeleteDataSetByIdAsync(id: string) {
        const url = `${this.relativeDataSetUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new DataSet(result.data));
    }
}

export default DataSetBroker;