import React, { FunctionComponent } from "react";
import { SubscriberAgreementView } from "../../models/views/components/subscriberAgreements/subscriberAgreement";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import { SecuredComponents } from "../links";
import securityPoints from "../../securityMatrix";
import ButtonBase from "../bases/buttons/ButtonBase";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faCopy, faTimes } from "@fortawesome/free-solid-svg-icons";
import moment from "moment";
import { Label } from "nhsuk-react-components";

interface SubscriberAgreementDetailCardViewProps {
    subscriberAgreement: SubscriberAgreementView;
    onDelete: (subscriberAgreement: SubscriberAgreementView) => void;
    mode: string;
    onModeChange: (value: string) => void;
}

const SubscriberAgreementDetailCardView: FunctionComponent<SubscriberAgreementDetailCardViewProps> = (props) => {
    const {
        subscriberAgreement,
        onDelete,
        mode,
        onModeChange
    } = props;

    const [ftpKeyCopied, setFtpKeyCopied] = React.useState<boolean>(false);
    const [gpgKeyCopied, setGpgKeyCopied] = React.useState<boolean>(false);


    return (
        <>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Agreement Short Name:</SummaryListBaseKey>
                    <div className="w-75">
                        {subscriberAgreement.supplierSharingAgreementShortName}
                    </div>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Ftp UserName</SummaryListBaseKey>
                    <div className="w-75">
                        {subscriberAgreement.ftpUserName}
                    </div>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Ftp Public Key&nbsp;
                        {ftpKeyCopied ?
                            <FontAwesomeIcon icon={faCheck} />
                            : <FontAwesomeIcon icon={faCopy} onClick={() => { navigator.clipboard.writeText(subscriberAgreement.ftpPublicKey); setFtpKeyCopied(true); }} />
                        }
                    </SummaryListBaseKey>
                    <div className="w-75">
                        {subscriberAgreement.ftpPublicKey}
                    </div>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Gpg Public Key&nbsp;
                        {gpgKeyCopied ?
                            <FontAwesomeIcon icon={faCheck} />
                            : <FontAwesomeIcon icon={faCopy} onClick={() => { navigator.clipboard.writeText(subscriberAgreement.gpgPublicKey); setGpgKeyCopied(true); }} />
                        }
                    </SummaryListBaseKey>
                    <div className="w-75">
                        {subscriberAgreement.gpgPublicKey}
                    </div>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Active</SummaryListBaseKey>
                    <div className="w-75">
                        {subscriberAgreement.isActive
                            ? <FontAwesomeIcon icon={faCheck} className="text-success" />
                            : <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                    </div>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Poll Start Date</SummaryListBaseKey>
                    <div>
                        {subscriberAgreement.lastPollStartDate ? moment(subscriberAgreement.lastPollStartDate?.toString()).format("Do-MMM-yyyy"):"Never Started Polling"}
                    </div>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Poll End Date</SummaryListBaseKey>
                    <div>{subscriberAgreement.lastPollEndDate ? moment(subscriberAgreement.lastPollEndDate?.toString()).format("Do-MMM-yyyy") : "Never Finished Polling"}</div>
                </SummaryListBaseRow>
            </SummaryListBase>

            {mode === 'VIEW' &&
                <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.edit}>
                    <ButtonBase onClick={() => onModeChange('EDIT')} edit>Edit</ButtonBase>
                </SecuredComponents>
            }
            <>
                {mode === 'VIEW' &&
                    <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.delete}>
                        <ButtonBase onClick={() => onModeChange('CONFIRMDELETE')} remove>Delete</ButtonBase>
                    </SecuredComponents>
                }
                {mode === 'CONFIRMDELETE' &&
                    <>
                        <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.delete}>
                            <>
                                <ButtonBase onClick={() => onModeChange('VIEW')} cancel>Cancel</ButtonBase>
                                <ButtonBase onClick={() => onDelete(subscriberAgreement)} remove>Yes, Delete</ButtonBase>
                            </>
                        </SecuredComponents>
                    </>
                }
            </>
        </>
    );
}

export default SubscriberAgreementDetailCardView;