import React, { FunctionComponent, useState } from "react";
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
import JSZip from "jszip";
import { Alert } from "react-bootstrap";
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
    const [confirmed, setConfirmed] = useState(false);

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
                    <SummaryListBaseKey>Supplier Id:</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {subscriberCredential.supplierId.toString()}
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
                    <SummaryListBaseKey>Ftp Public Key&nbsp;</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {ftpKeyCopied ?
                            <FontAwesomeIcon icon={faCheck} className="text-secondary" />
                            : <FontAwesomeIcon icon={faCopy} title="Copy Public Key" style={{ cursor: "pointer"}}
                                onClick={() => {
                                    const publicKey = decodeBase64(subscriberCredential.ftpPublicKey ?? "");
                                    navigator.clipboard.writeText(publicKey);
                                    setFtpKeyCopied(true);
                                }}
                            />
                        }  <span className="d-none">&nbsp;
                            {decodeBase64(subscriberCredential.ftpPublicKey ?? "")}</span>
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Gpg Public Key&nbsp;
                    </SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {gpgKeyCopied ?
                            <FontAwesomeIcon icon={faCheck} className="text-secondary" />
                            : <FontAwesomeIcon icon={faCopy} title="Copy GPG Key" style={{ cursor: "pointer" }}
                                onClick={() => {
                                    const gpgPublicKey = decodeBase64(subscriberCredential.gpgPublicKey ?? ""); 
                                    navigator.clipboard.writeText(gpgPublicKey);
                                    setGpgKeyCopied(true);
                                }}
                            />
                        } <span className="d-none"> &nbsp;
                            {decodeBase64(subscriberCredential.gpgPublicKey ?? "")}</span>
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Keys to send Emis Zip</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <ButtonBase
                            onClick={async () => {
                                const zip = new JSZip();
                                const ftpPublicKey = decodeBase64(subscriberCredential.ftpPublicKey ?? "");
                                const gpgPublicKey = decodeBase64(subscriberCredential.gpgPublicKey ?? "");

                                zip.file("ftpPublicKey.ssh", ftpPublicKey);
                                zip.file("gpgPublicKey.asc", gpgPublicKey);

                                const zipBlob = await zip.generateAsync({ type: "blob" });

                                const url = URL.createObjectURL(zipBlob);
                                const link = document.createElement("a");
                                link.href = url;
                                link.download = subscriberCredential.supplierSharingAgreementShortName +  "_keys.zip";
                                link.click();
                                URL.revokeObjectURL(url);
                            }}
                            view
                        >
                            Download Key Zip
                        </ButtonBase>
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
                            <Alert variant="danger">
                                <strong>
                                    WARNING: If you proceed with Re-Generation, ALL current keys will be
                                    permanently lost and cannot be recovered.
                                </strong>
                                <br /><br />
                            <label>
                                <input
                                    type="checkbox"
                                    onChange={(e) => setConfirmed(e.target.checked)}
                                />
                                &nbsp;I confirm that I understand and accept this outcome.
                                </label>
                            </Alert>
                            <ButtonBase onClick={() => onModeChange('VIEW')} cancel>Cancel</ButtonBase>

                            {confirmed && (
                                <ButtonBase onClick={() => onRegenerate(subscriberCredential)} view>Yes, Re-Generate</ButtonBase>
                            )}
                           
                        </>
                    </SecuredComponents>
                }
            </>
        </>
    );
}

export default SubscriberAgreementDetailCardView;
