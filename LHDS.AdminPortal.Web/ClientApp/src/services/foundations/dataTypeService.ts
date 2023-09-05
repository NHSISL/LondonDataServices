import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import dataTypeBroker from "../../brokers/apiBroker.datatypes";
import { dataType } from "../../models/dataTypes/dataType";

export const Service = {
    useCreatedataType: () => {
        const broker = new dataTypeBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((dataType: dataType) => {
            const date = new Date();
            dataType.createdDate = dataType.updatedDate = date;
            dataType.createdBy = dataType.updatedBy = msal.accounts[0].username;

            return broker.PostdataTypeAsync(dataType);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("dataTypeGetAll");
                    queryClient.invalidateQueries(["dataTypeGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAlldataType: (query: string) => {
        const broker = new dataTypeBroker();

        return useQuery(
            ["dataTypeGetAll", { query: query }],
            () => broker.GetAlldataTypesAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAlldataTypePages: (query: string) => {
        const broker = new dataTypeBroker();

        return useInfiniteQuery(
            ["dataTypeGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetdataTypeFirstPagesAsync(query)
                }
                return broker.GetdataTypeSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifydataType: () => {
        const broker = new dataTypeBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((dataType: dataType) => {
            const date = new Date();
            dataType.updatedDate = date;
            dataType.updatedBy = msal.accounts[0].username;

            return broker.PutdataTypeAsync();
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("dataTypeGetAll");
                    queryClient.invalidateQueries(["dataTypeGetById", { id: data.id }]);
                }
            });
    },
}