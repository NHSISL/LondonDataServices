import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";
import { ResolvedAddress } from "../models/resolvedAddresses/resolvedAddress";

class ResolvedAddressBroker {
    relativeResolvedAddressUrl = '/api/resolvedAddresses';
    relativeResolvedAddressOdataUrl = '/odata/resolvedAddresses'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((resolvedAddress: ResolvedAddress) => new ResolvedAddress(resolvedAddress));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostResolvedAddressAsync(resolvedAddress: ResolvedAddress) {
        return await this.apiBroker.PostAsync(this.relativeResolvedAddressUrl, resolvedAddress)
            .then(result => new ResolvedAddress(result.data));
    }

    async GetAllResolvedAddressesAsync(queryString: string) {
        const url = this.relativeResolvedAddressUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((resolvedAddress: ResolvedAddress) => new ResolvedAddress(resolvedAddress)));
    }

    async GetResolvedAddressFirstPagesAsync(query: string) {
        const url = this.relativeResolvedAddressOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetResolvedAddressSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetResolvedAddressByIdAsync(id: string) {
        const url = `${this.relativeResolvedAddressUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new ResolvedAddress(result.data));
    }

    async PutResolvedAddressAsync(resolvedAddress: ResolvedAddress) {
        return await this.apiBroker.PutAsync(this.relativeResolvedAddressUrl, resolvedAddress)
            .then(result => new ResolvedAddress(result.data));
    }

    async DeleteResolvedAddressByIdAsync(id: string) {
        const url = `${this.relativeResolvedAddressUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new ResolvedAddress(result.data));
    }
}

export default ResolvedAddressBroker;