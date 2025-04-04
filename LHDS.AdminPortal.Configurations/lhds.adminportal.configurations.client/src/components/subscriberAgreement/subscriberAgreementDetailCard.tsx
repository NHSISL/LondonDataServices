import React, { FunctionComponent, useState } from "react";
import SubscriberAgreementDetailCardView from "./subscriberAgreementDetailCardView";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import { useNavigate } from "react-router-dom";
import SubscriberAgreementDetailCardEdit from "./subscriberAgreementDetailCardEdit";
import { SubscriberCredentialView } from "../../models/views/components/subscriberCredentials/subscriberCredentialView";

interface SubscriberAgreementDetailCardProps {
    subscriberCredential: SubscriberCredentialView;
    mode: string;
    onAdd: (subscriberCredential: SubscriberCredentialView) => void;
    onUpdate: (subscriberCredential: SubscriberCredentialView) => void;
    onDelete: (subscriberCredential: SubscriberCredentialView) => void;
    onRegenerate: (subscriberCredential: SubscriberCredentialView) => void;
    children?: React.ReactNode;
}

const SubscriberAgreementDetailCard: FunctionComponent<SubscriberAgreementDetailCardProps> = (props) => {
    const {
        subscriberCredential,
        mode,
        onAdd,
        onUpdate,
        onDelete,
        onRegenerate,
        children
    } = props;

    const [displayMode, setDisplayMode] = useState<string>(mode);
    const [apiError, setApiError] = useState<any>({});

    const handleModeChange = (value: string) => {
        setDisplayMode(value);
    };

    const navigate = useNavigate();

    const handleAdd = async (subscriberCredential: SubscriberCredentialView) => {
        try {
            await onAdd(subscriberCredential);
            navigate('/configuration/subscriberAgreements');
        } catch (error) {
            setDisplayMode('EDIT');
        }
    };

    const handleUpdate = async (subscriberCredential: SubscriberCredentialView) => {
        try {
            await onUpdate(subscriberCredential);
            setDisplayMode('VIEW');
        } catch (error) {
            setApiError(error);
            setDisplayMode('EDIT');
        }
    };

    const handleDelete = (subscriberCredential: SubscriberCredentialView) => {
        onDelete(subscriberCredential);
        setDisplayMode('VIEW');
    };

    const handleRegenerate = async (subscriberCredential: SubscriberCredentialView) => {
        try {
            await onRegenerate(subscriberCredential);
            setDisplayMode('VIEW');
        } catch (error) {
            setApiError(error);
            setDisplayMode('VIEW');
        }
    };

    const handleCancel = () => {
        setApiError({});
    }

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        {displayMode === "ADD" ? "New Subscriber Agreements" : "Subscriber Agreement Details"}
                    </CardBaseTitle>
                    <CardBaseContent>
                        {(displayMode === "VIEW" || displayMode === "CONFIRMDELETE" || displayMode === "CONFIRMREGEN") && (
                            <SubscriberAgreementDetailCardView
                                onModeChange={handleModeChange}
                                subscriberCredential={subscriberCredential}
                                onDelete={handleDelete}
                                onRegenerate={handleRegenerate}
                                mode={displayMode}
                            />
                        )}
                        {(displayMode === "EDIT" || displayMode === "ADD") && (
                            <SubscriberAgreementDetailCardEdit
                                onModeChange={handleModeChange}
                                onAdd={handleAdd}
                                onUpdate={handleUpdate}
                                onCancel={handleCancel}
                                subscriberCredential={subscriberCredential}
                                mode={displayMode}
                                apiError={apiError}
                            />
                        )}

                        {children !== undefined && (
                            <>
                                <br />
                                {children}
                            </>
                        )}
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
}

export default SubscriberAgreementDetailCard;