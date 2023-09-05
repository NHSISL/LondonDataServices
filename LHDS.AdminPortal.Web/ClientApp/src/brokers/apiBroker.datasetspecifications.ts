import { Guid } from "guid-typescript";
import { DataSetSpecification } from "../models/dataSetSpecifications/dataSetSpecification";
import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";

class DataSetSpecificationBroker {
    relativeDataSetSpecificationUrl = '/api/dataSetSpecifications';
    relativeDataSetSpecificationOdataUrl = '/odata/dataSetSpecifications'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((dataSetSpecification: any) => new DataSetSpecification(dataSetSpecification));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

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

    async GetDataSetSpecificationFirstPagesAsync(query: string) {
        var url = this.relativeDataSetSpecificationOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }
}

export default DataSetSpecificationBroker;