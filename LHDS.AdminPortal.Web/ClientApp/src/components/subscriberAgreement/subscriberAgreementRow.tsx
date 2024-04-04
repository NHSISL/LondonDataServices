import React, { FunctionComponent } from "react";
import SubscriberRowView from "./subscriberAgreementRowView";
import { SubscriberCredential } from "../../models/subscriberCredentials/subscriberCredentials";

type SubscriberAgreementRowProps = {
    subscriberCredential: SubscriberCredential;
};

const SubscriberAgreementRow: FunctionComponent<SubscriberAgreementRowProps> = (props) => {
    const { subscriberCredential } = props;
    return (
        <SubscriberRowView
            key={subscriberCredential.id.toString()}
            subscriberCredential={subscriberCredential} />
    );
};

export default SubscriberAgreementRow;