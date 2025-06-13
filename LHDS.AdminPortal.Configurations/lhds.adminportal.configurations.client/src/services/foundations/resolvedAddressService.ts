import { useMsal } from "@azure/msal-react";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import ResolvedAddressBroker from "../../brokers/apiBroker.resolvedAddresses";
import { ResolvedAddress } from "../../models/resolvedAddresses/resolvedAddress";

export const resolvedAddressService = {
    useCreateResolvedAddress: () => {
        const broker = new ResolvedAddressBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (resolvedAddress: ResolvedAddress) => {
                const date = new Date();
                resolvedAddress.createdDate = resolvedAddress.updatedDate = date;
                resolvedAddress.createdBy = resolvedAddress.updatedBy = msal.accounts[0].username;

                return broker.PostResolvedAddressAsync(resolvedAddress);
            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["ResolvedAddressGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["ResolvedAddressGetById", { id: variables.id }] });
            }
        });
    },

    useRetrieveAllResolvedAddresses: (query: string) => {
        const broker = new ResolvedAddressBroker();

        return useQuery({
            queryKey: ["ResolvedAddressGetAll", { query: query }],
            queryFn: () => broker.GetAllResolvedAddressesAsync(query),
            staleTime: Infinity
        })
    },

    useRetrieveAllResolvedAddressPages: (query: string) => {
        const broker = new ResolvedAddressBroker();

        return useInfiniteQuery({
            queryKey: ["ResolvedAddressGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetResolvedAddressFirstPagesAsync(query)
                }
                return broker.GetResolvedAddressSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useModifyResolvedAddress: () => {
        const broker = new ResolvedAddressBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (resolvedAddress: ResolvedAddress) => {
                const date = new Date();
                resolvedAddress.updatedDate = date;
                resolvedAddress.updatedBy = msal.accounts[0].username;

                return broker.PutResolvedAddressAsync(resolvedAddress);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["ResolvedAddressGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["ResolvedAddressGetById", { id: data.id }] });
            }
        });
    },

    useRemoveResolvedAddress: () => {
        const broker = new ResolvedAddressBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return broker.DeleteResolvedAddressByIdAsync(id);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["ResolvedAddressGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["ResolvedAddressGetById", { id: data.id }] });
            }
        });
    },
}