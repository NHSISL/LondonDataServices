import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import DataTypeBroker from "../../brokers/apiBroker.datatypes";
import { DataType } from "../../models/dataTypes/dataType";

export const Service = {
    useCreateDataType: () => {
        const Broker = new DataTypeBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((: DataType) => {
            const date = new Date();
            .createdDate = .updatedDate = date;
            .createdBy = .updatedBy = msal.accounts[0].username;

            return Broker.PostDataTypeAsync();
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("DataTypeGetAll");
                    queryClient.invalidateQueries(["DataTypeGetById", { id: variables.id }]);
                }
            });
    },

    useGetAllDataType: (query: string) => {
        const Broker = new DataTypeBroker();

        return useQuery(
            ["DataTypeGetAll", { query: query }],
            () => Broker.GetAllDataTypeAsync(query),
            { staleTime: Infinity });
    },

    useGetAllDataTypePages: (query: string) => {
        const Broker = new DataTypeBroker();

        return useInfiniteQuery(
            ["DataTypeGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return Broker.GetDataTypeFirstPagesAsync(query)
                }
                return Broker.GetDataTypeSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useUpdateDataType: () => {
        const Broker = new DataTypeBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((: DataType) => {
            const date = new Date();
            .updatedDate = date;
            .updatedBy = msal.accounts[0].username;

            return Broker.PutDataTypeAsync();
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("DataTypeGetAll");
                    queryClient.invalidateQueries(["DataTypeGetById", { id: data.id }]);
                }
            });
    },

    useRemoveDataType: () => {
        const Broker = new DataTypeBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return Broker.DeleteDataTypeByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("DataTypeGetAll");
                    queryClient.invalidateQueries(["DataTypeGetById", { id: data.id }]);
                }
            });
    },
}