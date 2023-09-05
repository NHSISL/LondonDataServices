import { Guid } from "guid-typescript";
import { DataType } from "../models/dataTypes/dataType";
import ApiBroker from "./apiBroker";

class DataTypeBroker {
    relativeDataTypeUrl = '/api/DataTypes';

    private apiBroker: ApiBroker = new ApiBroker();

    async PostDataTypeAsync(dataType: DataType) {
        return await this.apiBroker.PostAsync(this.relativeDataTypeUrl, dataType)
            .then(result => new DataType(result.data));
    }
}

export default DataTypeBroker;
