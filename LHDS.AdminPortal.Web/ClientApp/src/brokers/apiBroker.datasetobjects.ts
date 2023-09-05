import { Guid } from "guid-typescript";
import { DataSetObject } from "../models/dataSetObjects/dataSetObject";
import ApiBroker from "./apiBroker";

class DataSetObjectBroker {
    relativeDataSetObjectUrl = '/api/DataSetObjects';

    private apiBroker: ApiBroker = new ApiBroker();

    async PostDataSetObjectAsync(dataSetObject: DataSetObject) {
        return await this.apiBroker.PostAsync(this.relativeDataSetObjectUrl, dataSetObject)
            .then(result => new DataSetObject(result.data));
    }
}

export default DataSetObjectBroker;
