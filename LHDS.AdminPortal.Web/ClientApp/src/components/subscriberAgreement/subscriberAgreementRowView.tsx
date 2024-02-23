import React, { FunctionComponent } from "react";
import { SubscriberAgreement } from "../../models/subscriberAgreements/subscriberAgreements";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import ButtonBase from "../bases/buttons/ButtonBase";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

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
            <TableBaseData>
                {subscriberAgreement.supplierSharingAgreementGuid!.toString()}
            </TableBaseData>
            <TableBaseData>
                <ButtonBase onClick={() => { }} add> Details</ButtonBase>
            </TableBaseData>
            
        </TableBaseRow>
    </>
    );
}

export default SubscriberRowView;