import { useMsal } from "@azure/msal-react";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import AddressBroker from "../../brokers/apiBroker.addresses";
import { Address } from "../../models/addresses/address";

export const addressService = {
    useCreateAddress: () => {
        const broker = new AddressBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (address: Address) => {
                const date = new Date();
                address.createdDate = address.updatedDate = date;
                address.createdBy = address.updatedBy = msal.accounts[0].username;

                return broker.PostAddressAsync(address);
            },

            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["AddressGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["AddressGetById", { id: variables.id }] });
            }
        });
    },

    useRetrieveAllAddresses: (query: string) => {
        const broker = new AddressBroker();

        return useQuery({
            queryKey: ["AddressGetAll", { query: query }],
            queryFn: () => broker.GetAllAddressesAsync(query),
            staleTime: Infinity
        });
    },

    useRetrieveAllAddressPages: (query: string) => {
        const broker = new AddressBroker();

        return useInfiniteQuery({
            queryKey: ["AddressGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetAddressFirstPagesAsync(query)
                }
                return broker.GetAddressSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useModifyAddress: () => {
        const broker = new AddressBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (address: Address) => {
                const date = new Date();
                address.updatedDate = date;
                address.updatedBy = msal.accounts[0].username;

                return broker.PutAddressAsync(address);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["AddressGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["AddressGetById", { id: data.id }] });
            }
        });
    },

    useRemoveAddress: () => {
        const broker = new AddressBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return broker.DeleteAddressByIdAsync(id);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["AddressGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["AddressGetById", { id: data.id }] });
            }
        });
    },
}