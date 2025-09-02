import React, { FunctionComponent } from "react";
import SubscriberRowView from "./subscriberAgreementRowView";
import { SubscriberCredential } from "../../models/subscriberCredentials/subscriberCredentials";
import { SupplierView } from "../../models/views/components/suppliers/supplierView"; // Adjust import path if needed

type SubscriberAgreementRowProps = {
    subscriberCredential: SubscriberCredential;
    suppliers: SupplierView[];
};

const SubscriberAgreementRow: FunctionComponent<SubscriberAgreementRowProps> = (props) => {
    const { subscriberCredential, suppliers } = props;
    return (
        <SubscriberRowView
            key={subscriberCredential.id.toString()}
            subscriberCredential={subscriberCredential}
            suppliers={suppliers}
        />
    );
};

export default SubscriberAgreementRow;