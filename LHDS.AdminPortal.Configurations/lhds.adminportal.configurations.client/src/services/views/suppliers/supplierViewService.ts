import { useEffect, useState } from "react";
import { Supplier } from "../../../models/suppliers/supplier";
import { SupplierView } from "../../../models/views/components/suppliers/supplierView";
import { supplierService } from "../../foundations/supplierService";


export const supplierViewService = {

    useCreateSupplier: () => {
        return supplierService.useCreateSupplier();
    },

    useGetAllSuppliers: (searchTerm?: string) => {
        let query = '?$orderby=Name';

        if (searchTerm) {
            query = query + `&$filter=contains(Name,'${searchTerm}')`;
        }

        const response = supplierService.useGetAllSuppliers(query);
        const [mappedSuppliers, setMappedSuppliers] = useState<Array<SupplierView>>([]);

        useEffect(() => {
            if (response.data) {
                const suppliers = response.data as Supplier[]
                setMappedSuppliers(suppliers);
            }
        }, [response.data]);

        return {
            mappedSuppliers, ...response
        }
    },

    useGetSupplierById: (id: string) => {
        const query = `?$filter=id eq ${id}`
        const response = supplierService.useGetAllSuppliers(query);
        const [mappedSupplier, setMappedSupplier] = useState<SupplierView>();

        useEffect(() => {
            if (response.data && response.data[0]) {
                const supplier = response.data as SupplierView
                setMappedSupplier(supplier);
            }
        }, [response.data]);

        return {
            mappedSupplier, ...response
        }
    },

    useUpdateSupplier: () => {
        return supplierService.useUpdateSupplier();
    },

    useRemoveSupplier: () => {
        return supplierService.useDeleteSupplier();
    },
}