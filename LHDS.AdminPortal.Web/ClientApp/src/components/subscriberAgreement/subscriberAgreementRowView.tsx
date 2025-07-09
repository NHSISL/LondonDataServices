import React, { FunctionComponent } from "react";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import ButtonBase from "../bases/buttons/ButtonBase";
import { Link } from "react-router-dom";
import { SubscriberCredential } from "../../models/subscriberCredentials/subscriberCredentials";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";

type SubscriberAgreementRowProps = {
    subscriberCredential: SubscriberCredential;
}

const SubscriberRowView: FunctionComponent<SubscriberAgreementRowProps> = (props) => {
    const {
        subscriberCredential
    } = props;

    return (<>

        <TableBaseRow>
            <TableBaseData>
                {subscriberCredential.id.toString()}
            </TableBaseData>
            <TableBaseData>
                {subscriberCredential.supplierSharingAgreementShortName}
            </TableBaseData>
            <TableBaseData>
                {subscriberCredential.supplierSharingAgreementGuid!.toString()}
            </TableBaseData>
            <TableBaseData>
                {subscriberCredential.isActive
                    ? <FontAwesomeIcon icon={faCheck} className="text-success" />
                    : <FontAwesomeIcon icon={faTimes} className="text-danger" />}
            </TableBaseData>
            <TableBaseData>
                <Link to={`/subscriberAgreementDetail/${subscriberCredential.id}`}>
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