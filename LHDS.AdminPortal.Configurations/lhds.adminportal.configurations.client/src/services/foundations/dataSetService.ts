import { useMsal } from "@azure/msal-react";
import DataSetBroker from "../../brokers/apiBroker.datasets";
import { DataSet } from "../../models/dataSets/dataSet";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export const dataSetService = {
    useCreateDataSet: () => {
        const broker = new DataSetBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (dataSet: DataSet) => {
                const date = new Date();
                dataSet.createdDate = dataSet.updatedDate = date;
                dataSet.createdBy = dataSet.updatedBy = msal.accounts[0].username;

                return broker.PostDataSetAsync(dataSet);
            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["DataSetGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["DataSetGetById", { id: variables.id }]});
            }
        });
    },

    useRetrieveAllDataSet: (query: string) => {
        const broker = new DataSetBroker();

        return useQuery({
            queryKey: ["DataSetGetAll", { query: query }],
            queryFn: () => broker.GetAllDataSetsAsync(query),
            staleTime: Infinity
        });
    },

    useRetrieveAllDataSetPages: (query: string) => {
        const broker = new DataSetBroker();

        return useInfiniteQuery({
            queryKey: ["DataSetGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetDataSetFirstPagesAsync(query)
                }
                return broker.GetDataSetSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useModifyDataSet: () => {
        const broker = new DataSetBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (dataSet: DataSet) => {
                const date = new Date();
                dataSet.updatedDate = date;
                dataSet.updatedBy = msal.accounts[0].username;

                return broker.PutDataSetAsync(dataSet);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["DataSetGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["DataSetGetById", { id: data.id }] });
            }
        });
    },

    useRemoveDataSet: () => {
        const broker = new DataSetBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return broker.DeleteDataSetByIdAsync(id);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["DataSetGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["DataSetGetById", { id: data.id }] });
            }
        });
    },
}