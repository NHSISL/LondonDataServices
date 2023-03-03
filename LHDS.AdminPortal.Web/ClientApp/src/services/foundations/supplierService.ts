import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useMutation, useQuery, useQueryClient } from "react-query";
import SupplierBroker from "../../brokers/apiBroker.suppliers";
import { Supplier } from "../../models/suppliers/supplier";

export const supplierService = {

    useCreateSupplier: () => {
        const supplierBroker = new SupplierBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((supplier: Supplier) => {
            const date = new Date();
            supplier.createdDate = supplier.updatedDate = date;
            supplier.createdBy = supplier.updatedBy = msal.accounts[0].username;

            return supplierBroker.PostSupplierAsync(supplier);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("SupplierGetAll");
                    queryClient.invalidateQueries(["SupplierGetById", { id: variables.id }]);
                }
            });
    },

    useGetAllSuppliers: (query: string) => {
        const supplierBroker = new SupplierBroker();

        return useQuery(
            ["SupplierGetAll", { query: query }],
            () => supplierBroker.GetAllSuppliersAsync(query),
            { staleTime: Infinity });
    },

    useGetSupplierById: (id: Guid) => {
        const supplierBroker = new SupplierBroker();

        return useQuery(
            ["SupplierGetById", { id: id }],
            () => supplierBroker.GetSupplierByIdAsync(id),
            { staleTime: Infinity });
    },

    useUpdateSupplier: () => {
        const supplierBroker = new SupplierBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((supplier: Supplier) => {
            const date = new Date();
            supplier.updatedDate = date;
            supplier.updatedBy = msal.accounts[0].username;

            return supplierBroker.PutSupplierAsync(supplier);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("SupplierGetAll");
                    queryClient.invalidateQueries(["SupplierGetById", { id: data.id }]);
                }
            });
    },

    useDeleteSupplier: () => {
        const supplierBroker = new SupplierBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return supplierBroker.DeleteSupplierByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("SupplierGetAll");
                    queryClient.invalidateQueries(["SupplierGetById", { id: data.id }]);
                }
            });
    },
}