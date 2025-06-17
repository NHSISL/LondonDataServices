import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import AddressBroker from "../../brokers/apiBroker.addresses";
import { Address } from "../../models/addresses/address";

export const addressService = {
    useCreateAddress: () => {
        const broker = new AddressBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((address: Address) => {
            const date = new Date();
            address.createdDate = address.updatedDate = date;
            address.createdBy = address.updatedBy = msal.accounts[0].username;

            return broker.PostAddressAsync(address);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("AddressGetAll");
                    queryClient.invalidateQueries(["AddressGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllAddresses: (query: string) => {
        const broker = new AddressBroker();

        return useQuery(
            ["AddressGetAll", { query: query }],
            () => broker.GetAllAddressesAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllAddressPages: (query: string) => {
        const broker = new AddressBroker();

        return useInfiniteQuery(
            ["AddressGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetAddressFirstPagesAsync(query)
                }
                return broker.GetAddressSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyAddress: () => {
        const broker = new AddressBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((address: Address) => {
            const date = new Date();
            address.updatedDate = date;
            address.updatedBy = msal.accounts[0].username;

            return broker.PutAddressAsync(address);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("AddressGetAll");
                    queryClient.invalidateQueries(["AddressGetById", { id: data.id }]);
                }
            });
    },

    useRemoveAddress: () => {
        const broker = new AddressBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteAddressByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("AddressGetAll");
                    queryClient.invalidateQueries(["AddressGetById", { id: data.id }]);
                }
            });
    },
}