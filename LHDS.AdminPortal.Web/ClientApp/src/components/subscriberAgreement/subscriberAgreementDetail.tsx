import React, { FunctionComponent, useEffect, useState } from "react";
import SubscriberAgreementDetailCard from "./subscriberAgreementDetailCard";
import { SubscriberAgreementView } from "../../models/views/components/subscriberAgreements/subscriberAgreement";
import { Guid } from "guid-typescript";
import { subscriberAgreementViewService } from "../../services/views/subscriberAgreements/subscriberAgreementViewService";

interface SubscriberAgreementDetailProps {
    subscriberAgreementId?: string;
    children?: React.ReactNode;
}

const SubscriberAgreementDetail: FunctionComponent<SubscriberAgreementDetailProps> = (props) => {
    const {
        subscriberAgreementId,
        children
    } = props;

    let subscriberAgreementRetrieved: SubscriberAgreementView | undefined

    if (subscriberAgreementId !== "" && subscriberAgreementId !== undefined) {
        let { mappedSubscriberAgreement } =
            subscriberAgreementViewService.useGetSubscriberAgreementById(Guid.parse(subscriberAgreementId ?? Guid.EMPTY));

        subscriberAgreementRetrieved = mappedSubscriberAgreement;
    }

    const [subscriberAgreement, setSubscriberAgreement] = useState<SubscriberAgreementView>();
    const [mode, setMode] = useState<string>('VIEW');

    const addSubscriberAgreement = subscriberAgreementViewService.useCreateSubscriberAgreement();

    const handleAdd = (subscriberAgreement: SubscriberAgreementView) => {
        return addSubscriberAgreement.mutate(subscriberAgreement);
    }

    const updateSubscriberAgreement = subscriberAgreementViewService.useUpdateSubscriberAgreement();

    const handleUpdate = async (subscriberAgreement:SubscriberAgreementView) => {
        return updateSubscriberAgreement.mutateAsync(subscriberAgreement);
    }

    const handleDelete = async (subscriberAgreement: SubscriberAgreementView) => {
        subscriberAgreement.isActive = false;
        return updateSubscriberAgreement.mutateAsync(subscriberAgreement);
    }

    useEffect(() => {
        if (subscriberAgreementId !== "" && subscriberAgreementRetrieved !== undefined) {
            setSubscriberAgreement(subscriberAgreementRetrieved);
            setMode('VIEW');
        }
        if (subscriberAgreementId === "" || subscriberAgreementId === undefined) {
            setSubscriberAgreement(new SubscriberAgreementView(Guid.create(), "", "", "", "",true))
            setMode('ADD');
        }
    }, [subscriberAgreementId, subscriberAgreementRetrieved]);

    return (
        <div>
            {subscriberAgreement !== undefined && (
                <div>
                    <SubscriberAgreementDetailCard
                        key={subscriberAgreement.id.toString()}
                        subscriberAgreement={subscriberAgreement}
                        mode={mode}
                        onAdd={handleAdd}
                        onUpdate={handleUpdate}
                        onDelete={handleDelete}>
                        {children}
                    </SubscriberAgreementDetailCard>
                </div>
            )}
        </div>
    );
}

export default SubscriberAgreementDetail;