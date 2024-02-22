import React, { FunctionComponent } from "react";
import { SubscriberAgreement } from "../../models/subscriberAgreements/subscriberAgreements";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";

type SubscriberAgreementRowProps = {
    subscriberAgreement: SubscriberAgreement;
}

const SubscriberRowView: FunctionComponent<SubscriberAgreementRowProps> = (props) => {
    const {
        subscriberAgreement
    } = props;

    return (<>

        <TableBaseRow>
            <TableBaseData>
                {subscriberAgreement.supplierSharingAgreementShortName}
            </TableBaseData>
            
        </TableBaseRow>
    </>
    );
}

export default SubscriberRowView;