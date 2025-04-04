import { useEffect, useState } from "react";
import { SubscriberAgreementView } from "../../../models/views/components/subscriberAgreements/subscriberAgreement";
import { subscriberAgreementService } from "../../foundations/subscriberAgreementService";
import { SubscriberAgreement } from "../../../models/subscriberAgreements/subscriberAgreements";


type SubscriberAgreementViewServiceResponse = {
    mappedSubscriberAgreements: SubscriberAgreementView[] | undefined;
    pages: any;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: any;
    refetch: () => void
}

export const subscriberAgreementViewService = {
    useCreateSubscriberAgreement: () => {
        return subscriberAgreementService.useCreateSubscriberAgreement();
    },

    useGetAllSubscriberAgreements: (searchTerm?: string): SubscriberAgreementViewServiceResponse => {

        let query = `?$orderby=createdDate desc`;

        if (searchTerm) {
            query = query + `&$filter=contains(supplierSharingAgreementShortName,'${searchTerm}')`;
        }

        const response = subscriberAgreementService.useRetrieveAllSubscriberAgreementPages(query);
        const [mappedSubscriberAgreements, setMappedSubscriberAgreements] = useState<Array<SubscriberAgreementView>>();
        const [pages, setPages] = useState<any>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const subscriberAgreements: Array<SubscriberAgreementView> = [];
                response.data.pages.forEach(x => {
                    x.data.forEach((subscriberAgreement: SubscriberAgreement) => {
                        subscriberAgreements.push(new SubscriberAgreementView(
                            subscriberAgreement.id,
                            subscriberAgreement.supplierSharingAgreementShortName,
                            subscriberAgreement.ftpUserName,
                            subscriberAgreement.ftpPublicKey,
                            subscriberAgreement.gpgPublicKey,
                            subscriberAgreement.isActive,
                            subscriberAgreement.supplierSharingAgreementGuid,
                            subscriberAgreement.lastPollStartDate,
                            subscriberAgreement.lastPollEndDate,
                            subscriberAgreement.createdBy,
                            subscriberAgreement.createdDate,
                            subscriberAgreement.updatedBy,
                            subscriberAgreement.updatedDate,
                        ));
                    });
                });

                setMappedSubscriberAgreements(subscriberAgreements);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedSubscriberAgreements,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },

    useGetSubscriberAgreementById: (id: string) => {
        const query = `?$filter=id eq ${id}`
        const response = subscriberAgreementService.useRetrieveAllSubscriberAgreement(query)
        const [mappedSubscriberAgreement, setMappedSubscriberAgreement] = useState<SubscriberAgreementView>();

        useEffect(() => {
            if (response.data && response.data[0]) {
                const subscriberAgreement = new SubscriberAgreementView(
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

                setMappedSubscriberAgreement(subscriberAgreement);
            }
        }, [response.data]);

        return {
            mappedSubscriberAgreement, ...response
        }
    },

    useUpdateSubscriberAgreement: () => {
        return subscriberAgreementService.useModifySubscriberAgreement();
    },

    useRemoveSubscriberAgreement: () => {
        return subscriberAgreementService.useRemoveSubscriberAgreement();
    },
};
