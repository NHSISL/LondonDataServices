
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import TerminologyArtifactBroker from "../../brokers/apiBroker.terminologyArtifacts";
import { TerminologyArtifact } from "../../models/terminologyArtifacts/terminologyArtifact";
import { useMsal } from "@azure/msal-react";

export const terminologyArtifactService = {

    useCreateTerminologyArtifact: () => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (terminologyArtifact: TerminologyArtifact) => {
                const date = new Date();
                terminologyArtifact.createdDate = terminologyArtifact.updatedDate = date;
                terminologyArtifact.createdBy = terminologyArtifact.updatedBy = msal.accounts[0].username;

                return terminologyArtifactBroker.PostTerminologyArtifactAsync(terminologyArtifact);
            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["TerminologyArtifactGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["TerminologyArtifactsGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["TerminologyArtifactGetById", { id: variables.id }] });
            }
        });
    },

    useGetAllTerminologyArtifacts: (query: string) => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();

        return useQuery({
            queryKey: ["TerminologyArtifactGetAll", { query: query }],
            queryFn: () => terminologyArtifactBroker.GetAllTerminologyArtifactsAsync(query),
            staleTime: Infinity
        });
    },

    useGetAllTerminologyArtifactsPages: (query: string) => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();

        return useInfiniteQuery({
            queryKey: ["TerminologyArtifactsGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return terminologyArtifactBroker.GetTerminologyArtifactsFirstPagesAsync(query)
                }
                return terminologyArtifactBroker.GetTerminologyArtifactsSubsequentPagesAsync(pageParam)
            },

            getNextPageParam: (lastPage) => lastPage.nextPage,
            staleTime: Infinity
        });
    },

    useGetTerminologyArtifactById: (id: string) => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();

        return useQuery({
            queryKey: ["TerminologyArtifactGetById", { id: id }],
            queryFn: () => terminologyArtifactBroker.GetTerminologyArtifactByIdAsync(id),
            staleTime: Infinity
        });
    },

    useUpdateTerminologyArtifact: () => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (terminologyArtifact: TerminologyArtifact) => {
                const date = new Date();
                terminologyArtifact.updatedDate = date;
                terminologyArtifact.updatedBy = msal.accounts[0].username;

                return terminologyArtifactBroker.PutTerminologyArtifactAsync(terminologyArtifact);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["TerminologyArtifactGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["TerminologyArtifactsGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["TerminologyArtifactGetById", { id: data.id }] });
            }
        });
    },

    useDeleteTerminologyArtifact: () => {
        const terminologyArtifactBroker = new TerminologyArtifactBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return terminologyArtifactBroker.DeleteTerminologyArtifactByIdAsync(id);
            },
            onSuccess: (data) => {
                    queryClient.invalidateQueries({ queryKey: ["TerminologyArtifactGetAll"] });
                    queryClient.invalidateQueries({ queryKey: ["TerminologyArtifactsGetAll"] });
                    queryClient.invalidateQueries({ queryKey: ["TerminologyArtifactGetById", { id: data.id }] });
                }
            });
    },
}