
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import PdsBroker from "../../brokers/apiBroker.pds";
import { Pds } from "../../models/pds/pds";
import { useMsal } from "@azure/msal-react";

export const pdsService = {

    useCreatePds: () => {
        const pdsBroker = new PdsBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (pds: Pds) => {
                const date = new Date();
                pds.createdDate = pds.updatedDate = date;
                pds.createdBy = pds.updatedBy = msal.accounts[0].username;

                return pdsBroker.PostPdsAsync(pds);
            },

            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["PdsGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["PdsGetById", { id: variables.id }] });
            }
        });
    },

    useGetAllPds: (query: string) => {
        const pdsBroker = new PdsBroker();

        return useQuery({
            queryKey: ["PdsGetAll", { query: query }],
            queryFn: () => pdsBroker.GetAllPdsAsync(query),
            staleTime: Infinity
        });
    },

    useGetAllPdsPages: (query: string) => {
        const pdsBroker = new PdsBroker();

        return useInfiniteQuery({
            queryKey: ["PdsGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return pdsBroker.GetPdsFirstPagesAsync(query)
                }
                return pdsBroker.GetPdsSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            getNextPageParam: (lastPage: { nextPage?: string }) => lastPage.nextPage ?? null,
            staleTime: Infinity
        });
    },

    useUpdatePds: () => {
        const pdsBroker = new PdsBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (pds: Pds) => {
                const date = new Date();
                pds.updatedDate = date;
                pds.updatedBy = msal.accounts[0].username;

                return pdsBroker.PutPdsAsync(pds);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["PdsGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["PdsGetById", { id: data.id }] });
            }
        });
    },

    useDeletePds: () => {
        const pdsBroker = new PdsBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return pdsBroker.DeletePdsByIdAsync(id);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["PdsGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["PdsGetById", { id: data.id }] });
            }
        });
    },
}