import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useMutation, useQuery, useQueryClient } from "react-query";
import OptOutBroker from "../../brokers/apiBroker.optout";
import { OptOut } from "../../models/optout/optout";

export const optOutService = {
    useCreateOptOut: () => {
        const optOutBroker = new OptOutBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((optout: OptOut) => {
            const date = new Date();
            optout.createdDate = optout.updatedDate = date;
            optout.createdBy = optout.updatedBy = msal.accounts[0].username;

            return optOutBroker.PostOptOutAsync(optout);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("OptOutGetAll");
                    queryClient.invalidateQueries(["OptOutGetById", { id: variables.id }]);
                }
            });
    },

    useGetAllOptOuts: (query: string) => {
        const optOutBroker = new OptOutBroker();

        return useQuery(
            ["OptOutGetAll", { query: query }],
            () => optOutBroker.GetAllOptOutsAsync(query),
            { staleTime: Infinity });
    },

    useGetOptOutById: (nhsNumber: string) => {
        const optOutBroker = new OptOutBroker();

        return useQuery(
            ["OptOutGetById", { nhsNumber: nhsNumber }],
            () => optOutBroker.GetOptOutByNhsNumberAsync(nhsNumber),
            { staleTime: Infinity });
    },

    useUpdateOptOut: () => {
        const optOutBroker = new OptOutBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((optOut: OptOut) => {
            const date = new Date();
            optOut.updatedDate = date;
            optOut.updatedBy = msal.accounts[0].username;

            return optOutBroker.PutOptOutAsync(optOut);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("OptOutGetAll");
                    queryClient.invalidateQueries(["OptOutGetById", { id: data.id }]);
                }
            });
    },

    useDeleteOptOut: () => {
        const optOutBroker = new OptOutBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return optOutBroker.DeleteOptOutByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("OptOutGetAll");
                    queryClient.invalidateQueries(["OptOutGetById", { id: data.id }]);
                }
            });
    },
}