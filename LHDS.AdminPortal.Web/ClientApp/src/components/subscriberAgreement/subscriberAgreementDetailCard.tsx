import React, { FunctionComponent, useState } from "react";
import SubscriberAgreementDetailCardView from "./subscriberAgreementDetailCardView";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import { SubscriberAgreementView } from "../../models/views/components/subscriberAgreements/subscriberAgreement";
import { useNavigate } from "react-router-dom";
import SubscriberAgreementDetailCardEdit from "./subscriberAgreementDetailCardEdit";

interface SubscriberAgreementDetailCardProps {
    subscriberAgreement: SubscriberAgreementView;
    mode: string;
    onAdd: (subscriberAgreement: SubscriberAgreementView) => void;
    onUpdate: (subscriberAgreement: SubscriberAgreementView) => void;
    onDelete: (subscriberAgreement: SubscriberAgreementView) => void;
    children?: React.ReactNode;
}

const SubscriberAgreementDetailCard: FunctionComponent<SubscriberAgreementDetailCardProps> = (props) => {
    const {
        subscriberAgreement,
        mode,
        onAdd,
        onUpdate,
        onDelete,
        children
    } = props;

    const [displayMode, setDisplayMode] = useState<string>(mode);
    const [apiError, setApiError] = useState<any>({});

    const handleModeChange = (value: string) => {
        setDisplayMode(value);
    };

    const navigate = useNavigate();

    const handleAdd = async (subscriberAgreement: SubscriberAgreementView) => {
        try {
            await onAdd(subscriberAgreement);
            navigate('/configuration/subscriberAgreements');
        } catch (error) {
            setDisplayMode('EDIT');
        }
    };

    const handleUpdate = async (subscriberAgreement: SubscriberAgreementView) => {
        try {
            await onUpdate(subscriberAgreement);
            setDisplayMode('VIEW');
        } catch (error) {
            setApiError(error);
            setDisplayMode('EDIT');
        }
    };

    const handleDelete = (subscriberAgreement: SubscriberAgreementView) => {
        onDelete(subscriberAgreement);
        setDisplayMode('VIEW');
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
                                subscriberAgreement={subscriberAgreement}
                                onDelete={handleDelete}
                                mode={displayMode}
                            />
                        )}
                        {(displayMode === "EDIT" || displayMode === "ADD") && (
                            <SubscriberAgreementDetailCardEdit
                                onModeChange={handleModeChange}
                                onAdd={handleAdd}
                                onUpdate={handleUpdate}
                                onCancel={handleCancel}
                                subscriberAgreement={subscriberAgreement}
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