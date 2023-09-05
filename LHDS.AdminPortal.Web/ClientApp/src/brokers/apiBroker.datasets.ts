import { Guid } from "guid-typescript";
import { DataSet } from "../models/dataSets/dataSet";
import ApiBroker from "./apiBroker";

class DataSetBroker {
    relativeDataSetUrl = '/api/DataSets';

    private apiBroker: ApiBroker = new ApiBroker();

    async PostDataSetAsync(dataSet: DataSet) {
        return await this.apiBroker.PostAsync(this.relativeDataSetUrl, dataSet)
            .then(result => new DataSet(result.data));
    }
}

export default DataSetBroker;
