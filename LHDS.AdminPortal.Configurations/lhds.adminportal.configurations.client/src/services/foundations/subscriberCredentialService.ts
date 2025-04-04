import { useMsal } from "@azure/msal-react";
import { SubscriberCredential } from "../../models/subscriberCredentials/subscriberCredentials";
import SubscriberCredentialBroker from "../../brokers/apiBroker.subscriberCredentials";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export const subscriberCredentialService = {
    useCreateSubscriberCredential: () => {
        const broker = new SubscriberCredentialBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (subscriberCredential: SubscriberCredential) => {
                const date = new Date();
                subscriberCredential.createdDate = subscriberCredential.updatedDate = date;
                subscriberCredential.createdBy = subscriberCredential.updatedBy = msal.accounts[0].username;

                return broker.PostSubscriberCredentialAndRegenerateKeysAsync(subscriberCredential);
            },

            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["SubscriberCredentialGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SubscriberCredentialGetById", { id: variables.id }] });
            }
        });
    },

    useRegenerateKeysSubscriberCredential: () => {
        const broker = new SubscriberCredentialBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mudtationFn: (subscriberCredential: SubscriberCredential) => {
                const date = new Date();
                subscriberCredential.createdDate = subscriberCredential.updatedDate = date;
                subscriberCredential.createdBy = subscriberCredential.updatedBy = msal.accounts[0].username;

                return broker.PostSubscriberCredentialAndRegenerateKeysAsync(subscriberCredential);

            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["SubscriberCredentialGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SubscriberCredentialGetById", { id: variables.id }] });
            }
        });
    },

    useRetrieveAllSubscriberCredential: (query: string) => {
        const broker = new SubscriberCredentialBroker();

        return useQuery({
            queryKey: ["SubscriberCredentialGetAll", { query: query }],
            queryFn: () => broker.GetAllSubscriberCredentialsAsync(query),
            staleTime: Infinity
        });
    },

    useRetrieveAllSubscriberCredentialPages: (query: string) => {
        const broker = new SubscriberCredentialBroker();

        return useInfiniteQuery({
            queryKey: ["SubscriberCredentialGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetSubscriberCredentialFirstPagesAsync(query)
                }
                return broker.GetSubscriberCredentialSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useModifySubscriberCredential: () => {
        const broker = new SubscriberCredentialBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (subscriberCredential: SubscriberCredential) => {
                const date = new Date();
                subscriberCredential.updatedDate = date;
                subscriberCredential.updatedBy = msal.accounts[0].username;

                return broker.PutSubscriberCredentialAsync(subscriberCredential);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["SubscriberCredentialGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SubscriberCredentialGetById", { id: data.id }] });
            }
        });
    },

    useRemoveSubscriberCredential: () => {
        const broker = new SubscriberCredentialBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return broker.DeleteSubscriberCredentialByIdAsync(id);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["SubscriberCredentialGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SubscriberCredentialGetById", { id: data.id }] });
            }
        });
    },
}