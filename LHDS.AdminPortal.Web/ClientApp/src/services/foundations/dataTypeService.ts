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
}