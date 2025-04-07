import { Supplier } from "../models/suppliers/supplier";
import ApiBroker from "./apiBroker";

class SupplierBroker {
    relativeSupplierUrl = '/api/suppliers';

    private apiBroker: ApiBroker = new ApiBroker();

    async PostSupplierAsync(supplier: Supplier) {
        return await this.apiBroker.PostAsync(this.relativeSupplierUrl, supplier)
            .then(result => new Supplier(result.data));
    }

    async GetAllSuppliersAsync(queryString: string) {
        const url = this.relativeSupplierUrl + queryString;

        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((supplier: Supplier) => new Supplier(supplier)));
    }

    async GetSupplierByIdAsync(id: string) {
        const url = `${this.relativeSupplierUrl}/${id}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new Supplier(result.data));
    }

    async PutSupplierAsync(supplier: Supplier) {
        return await this.apiBroker.PutAsync(this.relativeSupplierUrl, supplier)
            .then(result => new Supplier(result.data));
    }

    async DeleteSupplierByIdAsync(id: string) {
        const url = `${this.relativeSupplierUrl}/${id}`;

        return await this.apiBroker.DeleteAsync(url)
            .then(result => new Supplier(result.data));
    }
}
export default SupplierBroker;