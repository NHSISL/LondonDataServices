import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import PdsBroker from "../../brokers/apiBroker.pds";
import { Pds } from "../../models/pds/pds";

export const pdsService = {
    useCreatePds: () => {
        const pdsBroker = new PdsBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((pds: Pds) => {
            const date = new Date();
            pds.createdDate = pds.updatedDate = date;
            pds.createdBy = pds.updatedBy = msal.accounts[0].username;

            return pdsBroker.PostPdsAsync(pds);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("PdsGetAll");
                    queryClient.invalidateQueries(["PdsGetById", { id: variables.id }]);
                }
            });
    },

    useGetAllPds: (query: string) => {
        const pdsBroker = new PdsBroker();

        return useQuery(
            ["PdsGetAll", { query: query }],
            () => pdsBroker.GetAllPdsAsync(query),
            { staleTime: Infinity });
    },

    useGetAllPdsPages: (query: string) => {
        const pdsBroker = new PdsBroker();

        return useInfiniteQuery(
            ["PdsGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return pdsBroker.GetPdsFirstPagesAsync(query)
                }
                return pdsBroker.GetPdsSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useUpdatePds: () => {
        const pdsBroker = new PdsBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((pds: Pds) => {
            const date = new Date();
            pds.updatedDate = date;
            pds.updatedBy = msal.accounts[0].username;

            return pdsBroker.PutPdsAsync(pds);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("PdsGetAll");
                    queryClient.invalidateQueries(["PdsGetById", { id: data.id }]);
                }
            });
    },

    useDeletePds: () => {
        const pdsBroker = new PdsBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return pdsBroker.DeletePdsByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("PdsGetAll");
                    queryClient.invalidateQueries(["PdsGetById", { id: data.id }]);
                }
            });
    },
}