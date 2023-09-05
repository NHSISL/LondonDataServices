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

    async GetAllDataSetObjectsAsync(queryString: string) {
        const url = this.relativeDataSetObjectUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((optOut: any) => new DataSetObject(optOut)));
    }
}

export default DataSetObjectBroker;