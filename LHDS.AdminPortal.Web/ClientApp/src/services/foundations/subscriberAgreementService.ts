import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import SubscriberAgreementBroker from "../../brokers/apiBroker.subscriberagreement";
import { SubscriberAgreement } from "../../models/subscriberAgreements/subscriberAgreements";

export const subscriberAgreementService = {
    useCreateSubscriberAgreement: () => {
        const broker = new SubscriberAgreementBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((subscriberAgreement: SubscriberAgreement) => {
            const date = new Date();
            subscriberAgreement.createdDate = subscriberAgreement.updatedDate = date;
            v.createdBy = subscriberAgreement.updatedBy = msal.accounts[0].username;

            return broker.PostSubscriberAgreementAsync(subscriberAgreement);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("SubscriberAgreementGetAll");
                    queryClient.invalidateQueries(["SubscriberAgreementGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllSubscriberAgreement: (query: string) => {
        const broker = new SubscriberAgreementBroker();

        return useQuery(
            ["SubscriberAgreementGetAll", { query: query }],
            () => broker.GetAllSubscriberAgreementsAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllSubscriberAgreementPages: (query: string) => {
        const broker = new SubscriberAgreementBroker();

        return useInfiniteQuery(
            ["SubscriberAgreementGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetSubscriberAgreementFirstPagesAsync(query)
                }
                return broker.GetSubscriberAgreementSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifySubscriberAgreement: () => {
        const broker = new SubscriberAgreementBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((subscriberAgreement: SubscriberAgreement) => {
            const date = new Date();
            subscriberAgreement.updatedDate = date;
            subscriberAgreement.updatedBy = msal.accounts[0].username;

            return broker.PutSubscriberAgreementAsync(subscriberAgreement);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("SubscriberAgreementGetAll");
                    queryClient.invalidateQueries(["SubscriberAgreementGetById", { id: data.id }]);
                }
            });
    },

    useRemoveSubscriberAgreement: () => {
        const broker = new SubscriberAgreementBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteSubscriberAgreementByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("SubscriberAgreementGetAll");
                    queryClient.invalidateQueries(["SubscriberAgreementGetById", { id: data.id }]);
                }
            });
    },
}