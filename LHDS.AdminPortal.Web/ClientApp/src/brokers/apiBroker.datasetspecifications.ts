import { Guid } from "guid-typescript";
import { DataSetSpecification } from "../models/dataSetSpecifications/dataSetSpecification";
import ApiBroker from "./apiBroker";

class DataSetSpecificationBroker {
    relativeDataSetSpecificationUrl = '/api/DataSetSpecifications';

    private apiBroker: ApiBroker = new ApiBroker();

    async PostDataSetSpecificationAsync(dataSetSpecification: DataSetSpecification) {
        return await this.apiBroker.PostAsync(this.relativeDataSetSpecificationUrl, dataSetSpecification)
            .then(result => new DataSetSpecification(result.data));
    }

    async GetAllDataSetSpecificationsAsync(queryString: string) {
        const url = this.relativeDataSetSpecificationUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((optOut: any) => new DataSetSpecification(optOut)));
    }
}

export default DataSetSpecificationBroker;