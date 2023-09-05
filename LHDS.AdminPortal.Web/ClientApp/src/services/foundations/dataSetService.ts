import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import DataSetBroker from "../../brokers/apiBroker.datasets";
import { DataSet } from "../../models/dataSets/";

export const Service = {
    useCreateDataSet: () => {
        const Broker = new DataSetBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((: DataSet) => {
            const date = new Date();
            .createdDate = .updatedDate = date;
            .createdBy = .updatedBy = msal.accounts[0].username;

            return Broker.PostDataSetAsync();
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("DataSetGetAll");
                    queryClient.invalidateQueries(["DataSetGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllDataSet: (query: string) => {
        const Broker = new DataSetBroker();

        return useQuery(
            ["DataSetGetAll", { query: query }],
            () => Broker.GetAllDataSetAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllDataSetPages: (query: string) => {
        const Broker = new DataSetBroker();

        return useInfiniteQuery(
            ["DataSetGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return Broker.GetDataSetFirstPagesAsync(query)
                }
                return Broker.GetDataSetSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyDataSet: () => {
        const Broker = new DataSetBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((: DataSet) => {
            const date = new Date();
            .updatedDate = date;
            .updatedBy = msal.accounts[0].username;

            return Broker.PutDataSetAsync();
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("DataSetGetAll");
                    queryClient.invalidateQueries(["DataSetGetById", { id: data.id }]);
                }
            });
    },
}