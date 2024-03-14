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
import { faCheck, faCopy, faKey, faTimes } from "@fortawesome/free-solid-svg-icons";
import moment from "moment";

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

    React.useEffect(() => {
        const keyStateVariables = [
            { keyCopied: ftpKeyCopied, setKeyCopied: setFtpKeyCopied },
            { keyCopied: gpgKeyCopied, setKeyCopied: setGpgKeyCopied },
        ];

        keyStateVariables.forEach(({ keyCopied, setKeyCopied }) => {
            if (keyCopied) {
                const timerId = setTimeout(() => {
                    setKeyCopied(false);
                }, 5000);

                return () => clearTimeout(timerId);
            }
        });

    }, [ftpKeyCopied, gpgKeyCopied]);

    return (
        <>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Short Name:</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberAgreement.supplierSharingAgreementShortName}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Ftp UserName</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberAgreement.ftpUserName}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Ftp Public Key&nbsp;

                    </SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {ftpKeyCopied ?
                            <FontAwesomeIcon icon={faCheck} className="text-secondary" />
                            : <FontAwesomeIcon icon={faCopy} className="text-secondary"
                                onClick={() => {
                                    //navigator.clipboard.writeText(subscriberAgreement.ftpPublicKey);
                                    setFtpKeyCopied(true);
                                }} />
                        } &nbsp;
                        {subscriberAgreement.ftpPublicKey}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Gpg Public Key&nbsp;
                    </SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {gpgKeyCopied ?
                            <FontAwesomeIcon icon={faCheck} className="text-secondary" />
                            : <FontAwesomeIcon icon={faCopy} className="text-secondary"
                                onClick={() => {
                                    //navigator.clipboard.writeText(subscriberAgreement.gpgPublicKey);
                                    setGpgKeyCopied(true);
                                }} />
                        } &nbsp;
                        {subscriberAgreement.gpgPublicKey}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Active</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberAgreement.isActive
                            ? <FontAwesomeIcon icon={faCheck} className="text-success" />
                            : <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Poll Start Date</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {
                            subscriberAgreement.lastPollStartDate
                                ? moment(subscriberAgreement.lastPollStartDate?.toString()).format("Do-MMM-yyyy HH:mm")
                                : "Never Started Polling"
                        }
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Poll End Date</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberAgreement.lastPollEndDate ? moment(subscriberAgreement.lastPollEndDate?.toString()).format("Do-MMM-yyyy HH:mm") : "Never Finished Polling"}
                    </SummaryListBaseValue>
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

                {mode === 'VIEW' &&
                    <span className="float-end">
                        <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.edit}>
                            <ButtonBase onClick={() => onModeChange('CONFIRMREGEN')} info title={"Re-Generate Keys"}>
                                Re-Generate Keys &nbsp;
                                <FontAwesomeIcon icon={faKey}/>
                            </ButtonBase>
                        </SecuredComponents>
                    </span>
                }

                {mode === 'CONFIRMREGEN' &&
                    <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.delete}>
                        <>
                            <span className="text-danger">
                                <strong>NOTE: Continuing to regenerate will lose the current keys forever.</strong></span>
                            <br /> <br />
                            <ButtonBase onClick={() => onModeChange('VIEW')} cancel>Cancel</ButtonBase>
                            <ButtonBase onClick={() => onDelete(subscriberAgreement)} view>Yes, Re-Generate</ButtonBase>
                        </>
                    </SecuredComponents>
                }
            </>
        </>
    );
}

export default SubscriberAgreementDetailCardView;

function UseEffect(arg0: () => () => void, arg1: any[]) {
    throw new Error("Function not implemented.");
}
