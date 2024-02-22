import React, { FunctionComponent } from "react";
import SubscriberAgreementDetailCard from "./subscriberAgreementDetailCard";

interface SubscriberAgreementDetailProps {
    children?: React.ReactNode;
}

const SubscriberAgreementDetail: FunctionComponent<SubscriberAgreementDetailProps> = (props) => {
    const {
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