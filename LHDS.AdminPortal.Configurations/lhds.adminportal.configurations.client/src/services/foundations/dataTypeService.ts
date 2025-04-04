import { useMsal } from "@azure/msal-react";
import DataTypeBroker from "../../brokers/apiBroker.datatypes";
import { DataType } from "../../models/dataTypes/dataType";
import { useQueryClient, useMutation, useQuery, useInfiniteQuery } from "@tanstack/react-query";

export const dataTypeService = {
    useCreateDataType: () => {
        const broker = new DataTypeBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (dataType: DataType) => {
                const date = new Date();
                dataType.createdDate = dataType.updatedDate = date;
                dataType.createdBy = dataType.updatedBy = msal.accounts[0].username;

                return broker.PostDataTypeAsync(dataType);
            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["dataTypeGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["dataTypeGetById", { id: variables.id }] });
            }
        });
    },

    useRetrieveAllDataType: (query: string) => {
        const broker = new DataTypeBroker();

        return useQuery({
            queryKey: ["dataTypeGetAll", { query: query }],
            queryFn: () => broker.GetAllDataTypesAsync(query),
            staleTime: Infinity
        });
    },

    useRetrieveAllDataTypePages: (query: string) => {
        const broker = new DataTypeBroker();

        return useInfiniteQuery({
            queryKey: ["dataTypeGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetDataTypeFirstPagesAsync(query)
                }

                return broker.GetDataTypeSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useModifyDataType: () => {
        const broker = new DataTypeBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (dataType: DataType) => {
                const date = new Date();
                dataType.updatedDate = date;
                dataType.updatedBy = msal.accounts[0].username;

                return broker.PutDataTypeAsync(dataType);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["dataTypeGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["dataTypeGetById", { id: data.id }] });
            }
        });
    },

    useRemoveDataType: () => {
        const broker = new DataTypeBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return broker.DeleteDataTypeByIdAsync(id);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["dataTypeGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["dataTypeGetById", { id: data.id }] });
            }
        });
    },
}