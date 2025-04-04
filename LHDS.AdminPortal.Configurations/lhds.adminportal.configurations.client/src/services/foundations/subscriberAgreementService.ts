import { useMsal } from "@azure/msal-react";
import SubscriberAgreementBroker from "../../brokers/apiBroker.subscriberagreement";
import { SubscriberAgreement } from "../../models/subscriberAgreements/subscriberAgreements";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export const subscriberAgreementService = {

    useCreateSubscriberAgreement: () => {
        const broker = new SubscriberAgreementBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (subscriberAgreement: SubscriberAgreement) => {
                const date = new Date();
                subscriberAgreement.createdDate = subscriberAgreement.updatedDate = date;
                subscriberAgreement.createdBy = subscriberAgreement.updatedBy = msal.accounts[0].username;

                return broker.PostSubscriberAgreementAsync(subscriberAgreement);
            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["SubscriberAgreementGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SubscriberAgreementGetById", { id: variables.id }] });
            }
        });
    },

    useRetrieveAllSubscriberAgreement: (query: string) => {
        const broker = new SubscriberAgreementBroker();

        return useQuery({
            queryKey: ["SubscriberAgreementGetAll", { query: query }],
            queryFn: () => broker.GetAllSubscriberAgreementsAsync(query),
            staleTime: Infinity
        });
    },

    useRetrieveAllSubscriberAgreementPages: (query: string) => {
        const broker = new SubscriberAgreementBroker();

        return useInfiniteQuery({
            queryKey: ["SubscriberAgreementGetAll", { query: query }],
            queryFn: ({ pageParam = null }) => {
                if (!pageParam) {
                    return broker.GetSubscriberAgreementFirstPagesAsync(query);
                }
                return broker.GetSubscriberAgreementSubsequentPagesAsync(pageParam);
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useModifySubscriberAgreement: () => {
        const broker = new SubscriberAgreementBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (subscriberAgreement: SubscriberAgreement) => {
                const date = new Date();
                subscriberAgreement.updatedDate = date;
                subscriberAgreement.updatedBy = msal.accounts[0].username;

                return broker.PutSubscriberAgreementAsync(subscriberAgreement);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["SubscriberAgreementGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SubscriberAgreementGetById", { id: data.id }] });
            }
        });
    },

    useRemoveSubscriberAgreement: () => {
        const broker = new SubscriberAgreementBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return broker.DeleteSubscriberAgreementByIdAsync(id);
            },

            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["SubscriberAgreementGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SubscriberAgreementGetById", { id: data.id }] });
            }
        });
    },
}