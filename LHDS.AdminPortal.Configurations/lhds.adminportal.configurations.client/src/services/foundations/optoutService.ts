import { useMsal } from "@azure/msal-react";
import OptOutBroker from "../../brokers/apiBroker.optout";
import { OptOut } from "../../models/optout/optout";
import { useQueryClient, useMutation, useQuery } from "@tanstack/react-query";

export const optOutService = {
    useCreateOptOut: () => {
        const optOutBroker = new OptOutBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (optout: OptOut) => {
                const date = new Date();
                optout.createdDate = optout.updatedDate = date;
                optout.createdBy = optout.updatedBy = msal.accounts[0].username;

                return optOutBroker.PostOptOutAsync(optout);
            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["OptOutGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["OptOutGetById", { id: variables.id }] });
            }
        });
    },

    useGetAllOptOuts: (query: string) => {
        const optOutBroker = new OptOutBroker();

        return useQuery({
            queryKey: ["OptOutGetAll", { query: query }],
            queryFn: () => optOutBroker.GetAllOptOutsAsync(query),
            staleTime: Infinity
        });
    },

    useGetOptOutById: (nhsNumber: string) => {
        const optOutBroker = new OptOutBroker();

        return useQuery({
            queryKey: ["OptOutGetById", { nhsNumber: nhsNumber }],
            queryFn: () => optOutBroker.GetOptOutByNhsNumberAsync(nhsNumber),
            staleTime: Infinity
        });
    },

    useUpdateOptOut: () => {
        const optOutBroker = new OptOutBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (optOut: OptOut) => {
                const date = new Date();
                optOut.updatedDate = date;
                optOut.updatedBy = msal.accounts[0].username;

                return optOutBroker.PutOptOutAsync(optOut);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["OptOutGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["OptOutGetById", { id: data.id }] });
            }
        });
    },

    useDeleteOptOut: () => {
        const optOutBroker = new OptOutBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return optOutBroker.DeleteOptOutByIdAsync(id);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["OptOutGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["OptOutGetById", { id: data.id }] });
            }
        });
    },
}