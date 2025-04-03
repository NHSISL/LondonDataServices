import { useEffect, useState } from "react";
import { Supplier } from "../../../models/suppliers/supplier";
import { LookupView } from "../../../models/views/components/lookups/lookupView";
import { supplierService } from "../../foundations/supplierService";

export const lookupViewService = {
    useGetSupplierList: (searchTerm?: string) => {

        let query = `?$select=id,Name&$orderby=Name`;

        if (searchTerm) {
            query = query + `&$filter=contains(Name,'${searchTerm}')`
        }

        const response = supplierService.useGetAllSuppliers(query);
        const [mappedSuppliers, setMappedSuppliers] = useState<Array<LookupView>>([]);

        useEffect(() => {
            if (response.data) {
                const suppliers = response.data.map((supplier: Supplier) =>
                    new LookupView(
                        supplier.id.toString(),
                        supplier.name || ""
                    ));
                setMappedSuppliers(suppliers);
            }
        }, [response.data]);

        return {
            mappedSuppliers,
            ...response
        }

    },
};
