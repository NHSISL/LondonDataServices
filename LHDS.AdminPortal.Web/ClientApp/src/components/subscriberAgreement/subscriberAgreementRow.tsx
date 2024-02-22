import React, { FunctionComponent } from "react";
import { SubscriberAgreement } from "../../models/subscriberAgreements/subscriberAgreements";
import SubscriberRowView from "./subscriberAgreementRowView";

type SubscriberAgreementRowProps = {
    subscriberAgreement: SubscriberAgreement;
};

const SubscriberAgreementRow: FunctionComponent<SubscriberAgreementRowProps> = (props) => {
    const { subscriberAgreement } = props;
    return (
        <SubscriberRowView
            key={subscriberAgreement.id.toString()}
            subscriberAgreement={subscriberAgreement} />
    );
};

export default SubscriberAgreementRow;