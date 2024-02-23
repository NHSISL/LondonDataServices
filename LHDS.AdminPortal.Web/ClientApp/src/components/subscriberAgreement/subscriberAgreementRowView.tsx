import React, { FunctionComponent } from "react";
import { SubscriberAgreement } from "../../models/subscriberAgreements/subscriberAgreements";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import ButtonBase from "../bases/buttons/ButtonBase";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Link } from "react-router-dom";

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
                <Link to={`/subscriberAgreementDetail/${subscriberAgreement.id}`}>
                    <ButtonBase onClick={() => { }} add>
                        View/Edit
                    </ButtonBase>
                </Link>
            </TableBaseData>
        </TableBaseRow>
    </>
    );
}

export default SubscriberRowView;