import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import ResolvedAddressBroker from "../../brokers/apiBroker.resolvedAddresses";
import { ResolvedAddress } from "../../models/resolvedAddresses/resolvedAddress";

export const resolvedAddressService = {
    useCreateResolvedAddress: () => {
        const broker = new ResolvedAddressBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((resolvedAddress: ResolvedAddress) => {
            const date = new Date();
            resolvedAddress.createdDate = resolvedAddress.updatedDate = date;
            resolvedAddress.createdBy = resolvedAddress.updatedBy = msal.accounts[0].username;

            return broker.PostResolvedAddressAsync(resolvedAddress);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("ResolvedAddressGetAll");
                    queryClient.invalidateQueries(["ResolvedAddressGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllResolvedAddresses: (query: string) => {
        const broker = new ResolvedAddressBroker();

        return useQuery(
            ["ResolvedAddressGetAll", { query: query }],
            () => broker.GetAllResolvedAddressesAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllResolvedAddressPages: (query: string) => {
        const broker = new ResolvedAddressBroker();

        return useInfiniteQuery(
            ["ResolvedAddressGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetResolvedAddressFirstPagesAsync(query)
                }
                return broker.GetResolvedAddressSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyResolvedAddress: () => {
        const broker = new ResolvedAddressBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((resolvedAddress: ResolvedAddress) => {
            const date = new Date();
            resolvedAddress.updatedDate = date;
            resolvedAddress.updatedBy = msal.accounts[0].username;

            return broker.PutResolvedAddressAsync(resolvedAddress);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("ResolvedAddressGetAll");
                    queryClient.invalidateQueries(["ResolvedAddressGetById", { id: data.id }]);
                }
            });
    },

    useRemoveResolvedAddress: () => {
        const broker = new ResolvedAddressBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteResolvedAddressByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("ResolvedAddressGetAll");
                    queryClient.invalidateQueries(["ResolvedAddressGetById", { id: data.id }]);
                }
            });
    },
}