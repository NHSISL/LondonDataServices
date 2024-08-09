import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import TerminologyArtifactBroker from "../../brokers/apiBroker.terminologyArtifacts";
import { TerminologyArtifact } from "../../models/terminologyArtifacts/terminologyArtifact";

export const terminologyArtifactService = {

    useCreateTerminologyArtifact: () => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((terminologyArtifact: TerminologyArtifact) => {
            const date = new Date();
            terminologyArtifact.createdDate = terminologyArtifact.updatedDate = date;
            terminologyArtifact.createdBy = terminologyArtifact.updatedBy = msal.accounts[0].username;

            return terminologyArtifactBroker.PostTerminologyArtifactAsync(terminologyArtifact);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("TerminologyArtifactGetAll");
                    queryClient.invalidateQueries("TerminologyArtifactsGetAll");
                    queryClient.invalidateQueries(["TerminologyArtifactGetById", { id: variables.id }]);
                }
            });
    },

    useGetAllTerminologyArtifacts: (query: string) => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();

        return useQuery(
            ["TerminologyArtifactGetAll", { query: query }],
            () => terminologyArtifactBroker.GetAllTerminologyArtifactsAsync(query),
            { staleTime: Infinity });
    },

    useGetAllTerminologyArtifactsPages: (query: string) => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();

        return useInfiniteQuery(
            ["TerminologyArtifactsGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return terminologyArtifactBroker.GetTerminologyArtifactsFirstPagesAsync(query)
                }
                return terminologyArtifactBroker.GetTerminologyArtifactsSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useGetTerminologyArtifactById: (id: Guid) => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();

        return useQuery(
            ["TerminologyArtifactGetById", { id: id }],
            () => terminologyArtifactBroker.GetTerminologyArtifactByIdAsync(id),
            { staleTime: Infinity });
    },

    useUpdateTerminologyArtifact: () => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((terminologyArtifact: TerminologyArtifact) => {
            const date = new Date();
            terminologyArtifact.updatedDate = date;
            terminologyArtifact.updatedBy = msal.accounts[0].username;

            return terminologyArtifactBroker.PutTerminologyArtifactAsync(terminologyArtifact);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("TerminologyArtifactGetAll");
                    queryClient.invalidateQueries("TerminologyArtifactsGetAll");
                    queryClient.invalidateQueries(["TerminologyArtifactGetById", { id: data.id }]);
                }
            });
    },

    useDeleteTerminologyArtifact: () => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return terminologyArtifactBroker.DeleteTerminologyArtifactByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("TerminologyArtifactGetAll");
                    queryClient.invalidateQueries("TerminologyArtifactsGetAll");
                    queryClient.invalidateQueries(["TerminologyArtifactGetById", { id: data.id }]);
                }
            });
    },
}