import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import { SubscriberCredential } from "../../models/subscriberCredentials/subscriberCredentials";
import SubscriberCredentialBroker from "../../brokers/apiBroker.subscriberCredentials";

export const subscriberCredentialService = {
    useCreateSubscriberCredential: () => {
        const broker = new SubscriberCredentialBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((subscriberCredential: SubscriberCredential) => {
            const date = new Date();
            subscriberCredential.createdDate = subscriberCredential.updatedDate = date;
            subscriberCredential.createdBy = subscriberCredential.updatedBy = msal.accounts[0].username;

            return broker.PostSubscriberCredentialAndRegenerateKeysAsync(subscriberCredential);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("SubscriberCredentialGetAll");
                    queryClient.invalidateQueries(["SubscriberCredentialGetById", { id: variables.id }]);
                }
            });
    },

    useCreateSubscriberCredentialNoKeys: () => {
        const broker = new SubscriberCredentialBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((subscriberCredential: SubscriberCredential) => {
            const date = new Date();
            subscriberCredential.createdDate = subscriberCredential.updatedDate = date;
            subscriberCredential.createdBy = subscriberCredential.updatedBy = msal.accounts[0].username;

            return broker.PostSubscriberCredentialAsync(subscriberCredential);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("SubscriberCredentialGetAll");
                    queryClient.invalidateQueries(["SubscriberCredentialGetById", { id: variables.id }]);
                }
            });
    },

    useRegenerateKeysSubscriberCredential: () => {
        const broker = new SubscriberCredentialBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((subscriberCredential: SubscriberCredential) => {
            const date = new Date();
            subscriberCredential.createdDate = subscriberCredential.updatedDate = date;
            subscriberCredential.createdBy = subscriberCredential.updatedBy = msal.accounts[0].username;

            return broker.PostSubscriberCredentialAndRegenerateKeysAsync(subscriberCredential);
            
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("SubscriberCredentialGetAll");
                    queryClient.invalidateQueries(["SubscriberCredentialGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllSubscriberCredential: (query: string) => {
        const broker = new SubscriberCredentialBroker();

        return useQuery(
            ["SubscriberCredentialGetAll", { query: query }],
            () => broker.GetAllSubscriberCredentialsAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllSubscriberCredentialPages: (query: string) => {
        const broker = new SubscriberCredentialBroker();

        return useInfiniteQuery(
            ["SubscriberCredentialGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetSubscriberCredentialFirstPagesAsync(query)
                }
                return broker.GetSubscriberCredentialSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifySubscriberCredential: () => {
        const broker = new SubscriberCredentialBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((subscriberCredential: SubscriberCredential) => {
            const date = new Date();
            subscriberCredential.updatedDate = date;
            subscriberCredential.updatedBy = msal.accounts[0].username;

            return broker.PutSubscriberCredentialAsync(subscriberCredential);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("SubscriberCredentialGetAll");
                    queryClient.invalidateQueries(["SubscriberCredentialGetById", { id: data.id }]);
                }
            });
    },

    useRemoveSubscriberCredential: () => {
        const broker = new SubscriberCredentialBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteSubscriberCredentialByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("SubscriberCredentialGetAll");
                    queryClient.invalidateQueries(["SubscriberCredentialGetById", { id: data.id }]);
                }
            });
    },
}