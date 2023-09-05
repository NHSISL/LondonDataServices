import { Guid } from "guid-typescript";
import { DataType } from "../models/dataTypes/dataType";
import ApiBroker from "./apiBroker";

class DataTypeBroker {
    relativeDataTypeUrl = '/api/DataTypes';

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((dataType: any) => new DataType(dataType));

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
            .then(result => result.data.map((optOut: any) => new DataType(optOut)));
    }

    async GetDataTypeFirstPagesAsync(query: string) {
        var url = this.relativeDataTypeOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }
}

export default DataTypeBroker;