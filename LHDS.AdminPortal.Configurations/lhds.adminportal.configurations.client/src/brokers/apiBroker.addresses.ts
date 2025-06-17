import ApiBroker from "./apiBroker";
import { AxiosResponse } from "axios";
import { Address } from "../models/addresses/address";

class AddressBroker {
    relativeAddressUrl = '/api/addresses';
    relativeAddressOdataUrl = '/odata/addresses'

    private apiBroker: ApiBroker = new ApiBroker();

    private processOdataResult = (result: AxiosResponse) => {
        const data = result.data.value.map((address: Address) => new Address(address));

        const nextPage = result.data['@odata.nextLink'];
        return { data, nextPage }
    }

    async PostAddressAsync(address: Address) {
        return await this.apiBroker.PostAsync(this.relativeAddressUrl, address)
            .then(result => new Address(result.data));
    }

    async GetAllAddressesAsync(queryString: string) {
        const url = this.relativeAddressUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((address: Address) => new Address(address)));
    }

    async GetAddressFirstPagesAsync(query: string) {
        const url = this.relativeAddressOdataUrl + query;
        return this.processOdataResult(await this.apiBroker.GetAsync(url));
    }

    async GetAddressSubsequentPagesAsync(absoluteUri: string) {
        return this.processOdataResult(await this.apiBroker.GetAsyncAbsolute(absoluteUri));
    }

    async GetAddressByIdAsync(id: string) {
        const url = `${this.relativeAddressUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new Address(result.data));
    }

    async PutAddressAsync(address: Address) {
        return await this.apiBroker.PutAsync(this.relativeAddressUrl, address)
            .then(result => new Address(result.data));
    }

    async DeleteAddressByIdAsync(id: string) {
        const url = `${this.relativeAddressUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new Address(result.data));
    }
}

export default AddressBroker;