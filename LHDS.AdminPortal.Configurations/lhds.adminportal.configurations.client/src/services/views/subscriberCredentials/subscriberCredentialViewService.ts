import { useEffect, useState } from "react";
import { SubscriberCredentialView } from "../../../models/views/components/subscriberCredentials/subscriberCredentialView";
import { subscriberCredentialService } from "../../foundations/subscriberCredentialService";
import { SubscriberCredential } from "../../../models/subscriberCredentials/subscriberCredentials";


type SubscriberCredentialViewServiceResponse = {
    mappedSubscriberCredentials: SubscriberCredentialView[] | undefined;
    pages: Array<{ data: SubscriberCredentialView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: SubscriberCredentialView[] }> } | undefined;
    refetch: () => void
}

export const subscriberCredentialViewService = {
    useCreateSubscriberCredential: () => {
        return subscriberCredentialService.useCreateSubscriberCredential();
    },

    useRegenerateKeysSubscriberCredential: () => {
        return subscriberCredentialService.useRegenerateKeysSubscriberCredential();
    },

    useGetAllSubscriberCredentials: (searchTerm?: string): SubscriberCredentialViewServiceResponse => {
        let query = `?$orderby=createdDate desc`;

        if (searchTerm) {
            query = query + `&$filter=contains(supplierSharingAgreementShortName,'${searchTerm}')`;
        }

        const response = subscriberCredentialService.useRetrieveAllSubscriberCredentialPages(query);
        const [mappedSubscriberCredentials, setMappedSubscriberCredentials] = useState<Array<SubscriberCredentialView>>();
        const [pages, setPages] = useState<Array<{ data: SubscriberCredentialView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const subscriberCredentials: Array<SubscriberCredentialView> = [];
                response.data.pages.forEach(x => {
                    x.data.forEach((subscriberCredential: SubscriberCredential) => {
                        subscriberCredentials.push(new SubscriberCredentialView(
                            subscriberCredential.id,
                            subscriberCredential.supplierSharingAgreementShortName,
                            subscriberCredential.ftpUserName,
                            subscriberCredential.ftpPublicKey,
                            subscriberCredential.gpgPublicKey,
                            subscriberCredential.isActive,
                            subscriberCredential.supplierSharingAgreementGuid,
                            subscriberCredential.lastPollStartDate,
                            subscriberCredential.lastPollEndDate,
                            subscriberCredential.createdBy,
                            subscriberCredential.createdDate,
                            subscriberCredential.updatedBy,
                            subscriberCredential.updatedDate,
                        ));
                    });
                });

                setMappedSubscriberCredentials(subscriberCredentials);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedSubscriberCredentials,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },

    useGetSubscriberCredentialById: (id: string) => {
        const query = `?$filter=id eq ${id}`
        const response = subscriberCredentialService.useRetrieveAllSubscriberCredential(query)
        const [mappedSubscriberCredential, setMappedSubscriberCredential] = useState<SubscriberCredentialView>();

        useEffect(() => {
            if (response.data && response.data[0]) {
                const subscriberCredential = new SubscriberCredentialView(
                    response.data[0].id,
                    response.data[0].supplierSharingAgreementShortName,
                    response.data[0].ftpUserName,
                    response.data[0].ftpPublicKey,
                    response.data[0].gpgPublicKey,
                    response.data[0].isActive,
                    response.data[0].supplierSharingAgreementGuid,
                    response.data[0].lastPollStartDate,
                    response.data[0].lastPollEndDate,
                    response.data[0].createdBy,
                    response.data[0].createdDate,
                    response.data[0].updatedBy,
                    response.data[0].subscriberAgreement);

                setMappedSubscriberCredential(subscriberCredential);
            }
        }, [response.data]);

        return {
            mappedSubscriberCredential, ...response
        }
    },

    useUpdateSubscriberCredential: () => {
        return subscriberCredentialService.useModifySubscriberCredential();
    },

    useRemoveSubscriberCredential: () => {
        return subscriberCredentialService.useRemoveSubscriberCredential();
    },
};
