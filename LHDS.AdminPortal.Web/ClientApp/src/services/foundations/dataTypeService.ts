import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import DataTypeBroker from "../../brokers/apiBroker.datatypes";
import { DataType } from "../../models/dataTypes/dataType";

export const dataTypeService = {
    useCreateDataType: () => {
        const broker = new DataTypeBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((dataType: DataType) => {
            const date = new Date();
            dataType.createdDate = dataType.updatedDate = date;
            dataType.createdBy = dataType.updatedBy = msal.accounts[0].username;

            return broker.PostDataTypeAsync(dataType);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("dataTypeGetAll");
                    queryClient.invalidateQueries(["dataTypeGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllDataType: (query: string) => {
        const broker = new DataTypeBroker();

        return useQuery(
            ["dataTypeGetAll", { query: query }],
            () => broker.GetAllDataTypesAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllDataTypePages: (query: string) => {
        const broker = new DataTypeBroker();

        return useInfiniteQuery(
            ["dataTypeGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetDataTypeFirstPagesAsync(query)
                }

                return broker.GetDataTypeSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyDataType: () => {
        const broker = new DataTypeBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((dataType: DataType) => {
            const date = new Date();
            dataType.updatedDate = date;
            dataType.updatedBy = msal.accounts[0].username;

            return broker.PutDataTypeAsync(dataType);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("dataTypeGetAll");
                    queryClient.invalidateQueries(["dataTypeGetById", { id: data.id }]);
                }
            });
    },

    useRemoveDataType: () => {
        const broker = new DataTypeBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteDataTypeByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("dataTypeGetAll");
                    queryClient.invalidateQueries(["dataTypeGetById", { id: data.id }]);
                }
            });
    },
}