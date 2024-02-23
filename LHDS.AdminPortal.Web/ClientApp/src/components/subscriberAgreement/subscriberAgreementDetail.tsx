import React, { FunctionComponent } from "react";
import SubscriberAgreementDetailCard from "./subscriberAgreementDetailCard";

interface SubscriberAgreementDetailProps {
    subscriberAgreementId?: string;
    children?: React.ReactNode;
}

const SubscriberAgreementDetail: FunctionComponent<SubscriberAgreementDetailProps> = (props) => {
    const {
        subscriberAgreementId,
        children
    } = props;

    return (
        <div>
            <div>
                <SubscriberAgreementDetailCard>
                    {children}
                </SubscriberAgreementDetailCard>
            </div>
        </div>
    );
}

export default SubscriberAgreementDetail;