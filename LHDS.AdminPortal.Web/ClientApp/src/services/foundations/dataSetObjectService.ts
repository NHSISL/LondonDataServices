import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import DataSetObjectBroker from "../../brokers/apiBroker.datasetobjects";
import { DataSetObject } from "../../models/dataSetObjects/dataSetObject";

export const dataSetObjectService = {
    useCreateDataSetObject: () => {
        const broker = new DataSetObjectBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((dataSetObject: DataSetObject) => {
            const date = new Date();
            dataSetObject.createdDate = dataSetObject.updatedDate = date;
            dataSetObject.createdBy = dataSetObject.updatedBy = msal.accounts[0].username;

            return broker.PostDataSetObjectAsync(dataSetObject);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("DataSetObjectGetAll");
                    queryClient.invalidateQueries(["DataSetObjectGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllDataSetObject: (query: string) => {
        const broker = new DataSetObjectBroker();

        return useQuery(
            ["DataSetObjectGetAll", { query: query }],
            () => broker.GetAllDataSetObjectsAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllDataSetObjectPages: (query: string) => {
        const broker = new DataSetObjectBroker();

        return useInfiniteQuery(
            ["DataSetObjectGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetDataSetObjectFirstPagesAsync(query)
                }
                return broker.GetDataSetObjectSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyDataSetObject: () => {
        const broker = new DataSetObjectBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((dataSetObject: DataSetObject) => {
            const date = new Date();
            dataSetObject.updatedDate = date;
            dataSetObject.updatedBy = msal.accounts[0].username;

            return broker.PutDataSetObjectAsync(dataSetObject);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("DataSetObjectGetAll");
                    queryClient.invalidateQueries(["DataSetObjectGetById", { id: data.id }]);
                }
            });
    },

    useRemoveDataSetObject: () => {
        const broker = new DataSetObjectBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteDataSetObjectByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("DataSetObjectGetAll");
                    queryClient.invalidateQueries(["DataSetObjectGetById", { id: data.id }]);
                }
            });
    },
}