import { useMsal } from "@azure/msal-react";
import DataSetSpecificationBroker from "../../brokers/apiBroker.datasetspecifications";
import { DataSetSpecification } from "../../models/dataSetSpecifications/dataSetSpecification";
import { useQueryClient, useMutation, useQuery, useInfiniteQuery } from "@tanstack/react-query";

export const dataSetSpecificationService = {
    useCreateDataSetSpecification: () => {
        const broker = new DataSetSpecificationBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (dataSetSpecification: DataSetSpecification) => {
                const date = new Date();
                dataSetSpecification.createdDate = dataSetSpecification.updatedDate = date;
                dataSetSpecification.createdBy = dataSetSpecification.updatedBy = msal.accounts[0].username;

                return broker.PostDataSetSpecificationAsync(dataSetSpecification);
            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["DataSetSpecificationGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["DataSetSpecificationGetById", { id: variables.id }] });
            }
        });
    },

    useRetrieveAllDataSetSpecification: (query: string) => {
        const broker = new DataSetSpecificationBroker();

        return useQuery({
            queryKey: ["DataSetSpecificationGetAll", { query: query }],
            queryFn: () => broker.GetAllDataSetSpecificationsAsync(query),
            staleTime: Infinity
        });
    },

    useRetrieveAllDataSetSpecificationPages: (query: string) => {
        const broker = new DataSetSpecificationBroker();

        return useInfiniteQuery({
            queryKey: ["DataSetSpecificationGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetDataSetSpecificationFirstPagesAsync(query)
                }
                return broker.GetDataSetSpecificationSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useModifyDataSetSpecification: () => {
        const broker = new DataSetSpecificationBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (dataSetSpecification: DataSetSpecification) => {
                const date = new Date();
                dataSetSpecification.updatedDate = date;
                dataSetSpecification.updatedBy = msal.accounts[0].username;

                return broker.PutDataSetSpecificationAsync(dataSetSpecification);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["DataSetSpecificationGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["DataSetSpecificationGetById", { id: data.id }] });
            }
        });
    },

    useRemoveDataSetSpecification: () => {
        const broker = new DataSetSpecificationBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return broker.DeleteDataSetSpecificationByIdAsync(id);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["DataSetSpecificationGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["DataSetSpecificationGetById", { id: data.id }] });
            }
        });
    },
}