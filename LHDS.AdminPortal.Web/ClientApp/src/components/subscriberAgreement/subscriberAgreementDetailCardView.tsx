import React, { FunctionComponent } from "react";
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
import { SubscriberCredentialView } from "../../models/views/components/subscriberCredentials/subscriberCredentialView";

interface SubscriberAgreementDetailCardViewProps {
    subscriberCredential: SubscriberCredentialView;
    onDelete: (subscriberCredential: SubscriberCredentialView) => void;
    onRegenerate: (subscriberCredential: SubscriberCredentialView) => void;
    mode: string;
    onModeChange: (value: string) => void;
}

const SubscriberAgreementDetailCardView: FunctionComponent<SubscriberAgreementDetailCardViewProps> = (props) => {
    const {
        subscriberCredential,
        onDelete,
        onRegenerate,
        mode,
        onModeChange
    } = props;

    const [ftpKeyCopied, setFtpKeyCopied] = React.useState<boolean>(false);
    const [gpgKeyCopied, setGpgKeyCopied] = React.useState<boolean>(false);

    const decodeBase64 = (base64String: string) => {
        try 
        {
            return atob(base64String);
        } 
        catch (error) 
        {
            console.error('Failed to decode base64 string', error);
            return '';
        }
    };

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
                    <SummaryListBaseKey>Id:</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberCredential.id.toString()}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Short Name:</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberCredential.supplierSharingAgreementShortName}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Sharing Agreement Guid:</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberCredential.supplierSharingAgreementGuid!.toString()}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Ftp UserName</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberCredential.ftpUserName}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Ftp Public Key&nbsp;

                    </SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {ftpKeyCopied ?
                            <FontAwesomeIcon icon={faCheck} className="text-secondary" />
                            : <FontAwesomeIcon icon={faCopy}
                                onClick={() => {
                                    const publicKey = decodeBase64(subscriberCredential.ftpPublicKey ?? "");
                                    navigator.clipboard.writeText(publicKey);
                                    setFtpKeyCopied(true);
                                }}
                            />
                        } &nbsp;
                        {decodeBase64(subscriberCredential.ftpPublicKey ?? "")}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Gpg Public Key&nbsp;
                    </SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {gpgKeyCopied ?
                            <FontAwesomeIcon icon={faCheck} className="text-secondary" />
                            : <FontAwesomeIcon icon={faCopy}
                                onClick={() => {
                                    const gpgPublicKey = decodeBase64(subscriberCredential.gpgPublicKey ?? ""); 
                                    navigator.clipboard.writeText(gpgPublicKey);
                                    setGpgKeyCopied(true);
                                }}
                            />
                        } &nbsp;
                        {decodeBase64(subscriberCredential.gpgPublicKey ?? "")}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Active</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberCredential.isActive
                            ? <FontAwesomeIcon icon={faCheck} className="text-success" />
                            : <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Poll Start Date</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {
                            subscriberCredential.lastPollStartDate
                                ? moment(subscriberCredential.lastPollStartDate?.toString()).format("Do-MMM-yyyy HH:mm")
                                : "Never Started Polling"
                        }
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Poll End Date</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberCredential.lastPollEndDate ? moment(subscriberCredential.lastPollEndDate?.toString()).format("Do-MMM-yyyy HH:mm") : "Never Finished Polling"}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
            </SummaryListBase>

            {mode === 'VIEW' &&
                <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.edit}>
                    <ButtonBase onClick={() => onModeChange('EDIT')} edit>Edit</ButtonBase>
                </SecuredComponents>
            }
            <>
                {mode === 'VIEW'
                    //&&
                    //<SecuredComponents allowedRoles={securityPoints.subscriberAgreement.delete}>
                    //    <ButtonBase onClick={() => onModeChange('CONFIRMDELETE')} remove>Delete</ButtonBase>
                    //</SecuredComponents>
                }
                {mode === 'CONFIRMDELETE' &&
                    <>                        <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.delete}>
                        <>
                            <ButtonBase onClick={() => onModeChange('VIEW')} cancel>Cancel</ButtonBase>
                            <ButtonBase onClick={() => onDelete(subscriberCredential)} remove>Yes, Delete</ButtonBase>
                        </>
                    </SecuredComponents>
                    </>
                }

                {mode === 'VIEW' &&
                    <span className="float-end">
                        <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.edit}>
                            <ButtonBase onClick={() => onModeChange('CONFIRMREGEN')} info title={"Re-Generate Keys"}>
                                Re-Generate Keys &nbsp;
                                <FontAwesomeIcon icon={faKey} />
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
                            <ButtonBase onClick={() => onRegenerate(subscriberCredential)} view>Yes, Re-Generate</ButtonBase>
                        </>
                    </SecuredComponents>
                }
            </>
        </>
    );
}

export default SubscriberAgreementDetailCardView;
