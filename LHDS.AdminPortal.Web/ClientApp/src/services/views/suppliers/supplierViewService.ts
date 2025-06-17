import { Guid } from "guid-typescript";
import { useEffect, useState } from "react";
import { Supplier } from "../../../models/suppliers/supplier";
import { SupplierView } from "../../../models/views/components/suppliers/supplierView";
import { supplierService } from "../../foundations/supplierService";


export const supplierViewService = {

    useCreateSupplier: () => {
        return supplierService.useCreateSupplier();
    },

    useGetAllSuppliers: (searchTerm?: string) => {
        try {
            let query = '?$orderby=Name';

            if (searchTerm) {
                query = query + `&$filter=contains(Name,'${searchTerm}')`;
            }

            const response = supplierService.useGetAllSuppliers(query);
            const [mappedSuppliers, setMappedSuppliers] = useState<Array<SupplierView>>([]);

            useEffect(() => {
                if (response.data) {
                    const suppliers = response.data.map((supplier: Supplier) =>
                        new SupplierView(
                            supplier.id,
                            supplier.name,
                            supplier.friendlyName,
                            supplier.description,
                            supplier.isIngestionTracked,
                            supplier.canDecryptIngestionTracking,
                            supplier.createdBy,
                            supplier.createdDate,
                            supplier.updatedBy,
                            supplier.updatedDate,
                        ));

                    setMappedSuppliers(suppliers);
                }
            }, [response.data]);

            return {
                mappedSuppliers, ...response
            }
        } catch (err) {
            throw err;
        }
    },

    useGetSupplierById: (id: Guid) => {
        try {
            const query = `?$filter=id eq ${id}`
            const response = supplierService.useGetAllSuppliers(query);
            const [mappedSupplier, setMappedSupplier] = useState<SupplierView>();

            useEffect(() => {
                if (response.data && response.data[0]) {
                    const supplier = new SupplierView(
                        response.data[0].id,
                        response.data[0].name,
                        response.data[0].friendlyName,
                        response.data[0].description,
                        response.data[0].isIngestionTracked,
                        response.data[0].canDecryptIngestionTracking,
                        response.data[0].createdBy,
                        response.data[0].createdDate,
                        response.data[0].updatedBy,
                        response.data[0].updatedDate
                    );

                    setMappedSupplier(supplier);
                }
            }, [response.data]);

            return {
                mappedSupplier, ...response
            }
        } catch (err) {
            throw err;
        }
    },

    useUpdateSupplier: () => {
        return supplierService.useUpdateSupplier();
    },

    useRemoveSupplier: () => {
        return supplierService.useDeleteSupplier();
    },
}