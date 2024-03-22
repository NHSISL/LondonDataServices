import React, { FunctionComponent, useEffect, useState } from "react";
import SubscriberAgreementDetailCard from "./subscriberAgreementDetailCard";
import { SubscriberAgreementView } from "../../models/views/components/subscriberAgreements/subscriberAgreement";
import { Guid } from "guid-typescript";
import { subscriberAgreementViewService } from "../../services/views/subscriberAgreements/subscriberAgreementViewService";
import { subscriberCredentialViewService } from "../../services/views/subscriberCredentials/subscriberCredentialViewService";
import { SubscriberCredentialView } from "../../models/views/components/subscriberCredentials/subscriberCredentialView";

interface SubscriberAgreementDetailProps {
    subscriberAgreementId?: string;
    children?: React.ReactNode;
}

const SubscriberAgreementDetail: FunctionComponent<SubscriberAgreementDetailProps> = (props) => {
    const {
        subscriberAgreementId,
        children
    } = props;

    let subscriberCredentialRetrieved: SubscriberCredentialView | undefined

    if (subscriberAgreementId !== "" && subscriberAgreementId !== undefined) {
        let { mappedSubscriberCredential } =
            subscriberCredentialViewService.useGetSubscriberCredentialById(Guid.parse(subscriberAgreementId ?? Guid.EMPTY));

        subscriberCredentialRetrieved = mappedSubscriberCredential;
    }

    const [subscriberCredential, setSubscriberCredential] = useState<SubscriberCredentialView>();
    const [mode, setMode] = useState<string>('VIEW');

    const addSubscriberCredential = subscriberCredentialViewService.useCreateSubscriberCredential();

    const handleAdd = (subscriberCredential: SubscriberCredentialView) => {
        return addSubscriberCredential.mutate(subscriberCredential);
    }

    const handleRegenerate = (subscriberCredential: SubscriberCredentialView) => {
        return addSubscriberCredential.mutate(subscriberCredential);
    }

    const updateSubscriberCredential = subscriberCredentialViewService.useUpdateSubscriberCredential();

    const handleUpdate = async (subscriberCredential:SubscriberCredentialView) => {
        return updateSubscriberCredential.mutateAsync(subscriberCredential);
    }

    const handleDelete = async (subscriberCredential: SubscriberCredentialView) => {
        subscriberCredential.isActive = false;
        return updateSubscriberCredential.mutateAsync(subscriberCredential);
    }

    useEffect(() => {
        if (subscriberAgreementId !== "" && subscriberCredentialRetrieved !== undefined) {
            setSubscriberCredential(subscriberCredentialRetrieved);
            setMode('VIEW');
        }
        if (subscriberAgreementId === "" || subscriberAgreementId === undefined) {
            setSubscriberCredential(new SubscriberCredentialView(Guid.create(), "", "", "", "",true))
            setMode('ADD');
        }
    }, [subscriberAgreementId, subscriberCredentialRetrieved]);

    return (
        <div>
            {subscriberCredential !== undefined && (
                <div>
                    <SubscriberAgreementDetailCard
                        key={subscriberCredential.id.toString()}
                        subscriberCredential={subscriberCredential}
                        mode={mode}
                        onAdd={handleAdd}
                        onUpdate={handleUpdate}
                        onDelete={handleDelete}
                        onRegenerate={handleRegenerate}>
                        {children}
                    </SubscriberAgreementDetailCard>
                </div>
            )}
        </div>
    );
}

export default SubscriberAgreementDetail;