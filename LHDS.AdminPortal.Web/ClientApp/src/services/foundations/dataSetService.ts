import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import DataSetBroker from "../../brokers/apiBroker.datasets";
import { DataSet } from "../../models/dataSets/dataSet";

export const Service = {
    useCreateDataSet: () => {
        const broker = new DataSetBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((dataSet: DataSet) => {
            const date = new Date();
            dataSet.createdDate = dataSet.updatedDate = date;
            dataSet.createdBy = dataSet.updatedBy = msal.accounts[0].username;

            return broker.PostDataSetAsync(dataSet);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("DataSetGetAll");
                    queryClient.invalidateQueries(["DataSetGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllDataSet: (query: string) => {
        const broker = new DataSetBroker();

        return useQuery(
            ["DataSetGetAll", { query: query }],
            () => broker.GetAllDataSetsAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllDataSetPages: (query: string) => {
        const broker = new DataSetBroker();

        return useInfiniteQuery(
            ["DataSetGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetDataSetFirstPagesAsync(query)
                }
                return broker.GetDataSetSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },
}