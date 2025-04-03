import { useMsal } from "@azure/msal-react";
import SupplierBroker from "../../brokers/apiBroker.suppliers";
import { Supplier } from "../../models/suppliers/supplier";
import { useQueryClient, useMutation, useQuery } from "@tanstack/react-query";

export const supplierService = {

    useCreateSupplier: () => {
        const supplierBroker = new SupplierBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (supplier: Supplier) => {
                const date = new Date();
                supplier.createdDate = supplier.updatedDate = date;
                supplier.createdBy = supplier.updatedBy = msal.accounts[0].username;

                return supplierBroker.PostSupplierAsync(supplier);
            },

            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["SupplierGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SupplierGetById", { id: variables.id }] });
            }
        });
    },

    useGetAllSuppliers: (query: string) => {
        const supplierBroker = new SupplierBroker();

        return useQuery({
            queryKey: ["SupplierGetAll", { query: query }],
            queryFn: () => supplierBroker.GetAllSuppliersAsync(query),
            staleTime: Infinity
        });
    },

    useGetSupplierById: (id: string) => {
        const supplierBroker = new SupplierBroker();

        return useQuery({
            queryKey: ["SupplierGetById", { id: id }],
            queryFn: () => supplierBroker.GetSupplierByIdAsync(id),
            staleTime: Infinity
        });
    },

    useUpdateSupplier: () => {
        const supplierBroker = new SupplierBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (supplier: Supplier) => {
                const date = new Date();
                supplier.updatedDate = date;
                supplier.updatedBy = msal.accounts[0].username;

                return supplierBroker.PutSupplierAsync(supplier);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["SupplierGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SupplierGetById", { id: data.id }] });
            }
        });
    },

    useDeleteSupplier: () => {
        const supplierBroker = new SupplierBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return supplierBroker.DeleteSupplierByIdAsync(id);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["SupplierGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SupplierGetById", { id: data.id }] });
            }
        });
    },
}