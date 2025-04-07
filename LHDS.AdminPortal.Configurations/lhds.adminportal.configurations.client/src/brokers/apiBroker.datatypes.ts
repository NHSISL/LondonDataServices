import { DataType } from "../models/dataTypes/dataType";
import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";

class DataTypeBroker {
    relativeDataTypeUrl = '/api/dataTypes';
    relativeDataTypeOdataUrl = '/odata/dataTypes'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((dataType: DataType) => new DataType(dataType));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostDataTypeAsync(dataType: DataType) {
        return await this.apiBroker.PostAsync(this.relativeDataTypeUrl, dataType)
            .then(result => new DataType(result.data));
    }

    async GetAllDataTypesAsync(queryString: string) {
        const url = this.relativeDataTypeUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((dataType: DataType) => new DataType(dataType)));
    }

    async GetDataTypeFirstPagesAsync(query: string) {
        const url = this.relativeDataTypeOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetDataTypeSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetDataTypeByIdAsync(id: string) {
        const url = `${this.relativeDataTypeUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new DataType(result.data));
    }

    async PutDataTypeAsync(dataType: DataType) {
        return await this.apiBroker.PutAsync(this.relativeDataTypeUrl, dataType)
            .then(result => new DataType(result.data));
    }

    async DeleteDataTypeByIdAsync(id: string) {
        const url = `${this.relativeDataTypeUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new DataType(result.data));
    }
}

export default DataTypeBroker;