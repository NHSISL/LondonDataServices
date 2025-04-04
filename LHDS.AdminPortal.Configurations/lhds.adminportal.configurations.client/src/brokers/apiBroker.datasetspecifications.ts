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

    async GetDataSetSpecificationSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetDataSetSpecificationByIdAsync(id: string) {
        const url = `${this.relativeDataSetSpecificationUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new DataSetSpecification(result.data));
    }

    async PutDataSetSpecificationAsync(dataSetSpecification: DataSetSpecification) {
        return await this.apiBroker.PutAsync(this.relativeDataSetSpecificationUrl, dataSetSpecification)
            .then(result => new DataSetSpecification(result.data));
    }

    async DeleteDataSetSpecificationByIdAsync(id: string) {
        const url = `${this.relativeDataSetSpecificationUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new DataSetSpecification(result.data));
    }
}

export default DataSetSpecificationBroker;