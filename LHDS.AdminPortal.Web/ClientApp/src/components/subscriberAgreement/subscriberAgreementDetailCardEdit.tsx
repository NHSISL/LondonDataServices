import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import { SecuredComponents } from "../links";
import securityPoints from "../../securityMatrix";
import { useValidation } from "../../hooks/useValidation";
import TextInputBase from "../bases/inputs/TextInputBase";
import { SubscriberAgreementView } from "../../models/views/components/subscriberAgreements/subscriberAgreement";
import { subscriberAgreementErrors } from "./subscriberAgreementErrors";
import { subscriberAgreementValidation } from "./subscriberAgreementValidation";
import CheckboxBase from "../bases/inputs/CheckboxBase";

interface SubscriberAgreementDetailCardEditProps {
    subscriberAgreement: SubscriberAgreementView;
    onUpdate: (subscriberAgreement: SubscriberAgreementView) => void;
    onCancel: () => void;
    mode: string;
    onModeChange: (value: string) => void;
    apiError?: any;
}

const SubscriberAgreementDetailCardEdit: FunctionComponent<SubscriberAgreementDetailCardEditProps> = (props) => {
    const {
        subscriberAgreement,
        onUpdate,
        onCancel,
        mode,
        onModeChange,
        apiError
    } = props;

    const [editSubscriberAgreement, setEditSubscriberAgreement] = useState<SubscriberAgreementView>({ ...subscriberAgreement });

    const { errors, enableValidationMessages, processApiErrors, validate } = useValidation(
        subscriberAgreementErrors,
        subscriberAgreementValidation,
        editSubscriberAgreement)

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement> | ChangeEvent<HTMLSelectElement>) => {
        const updatedSubscriberAgreement = {
            ...editSubscriberAgreement,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setEditSubscriberAgreement(updatedSubscriberAgreement);
    };

    const handleCancel = () => {
        setEditSubscriberAgreement({ ...subscriberAgreement });
        onModeChange('VIEW')
        onCancel();
    };

    const handleUpdate = () => {
        if (!validate(editSubscriberAgreement)) {
            onUpdate(editSubscriberAgreement)
        } else {
            enableValidationMessages();
        }
    }

    useEffect(() => {
        processApiErrors(apiError)
    }, [apiError, processApiErrors])

    return (
        <>
            <SummaryListBase>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Subscriber Agreement Short Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="subscriberAgreementName"
                            name="subscriberAgreementName"
                            placeholder="DataSet Short Name"
                            required={true}
                            value={editSubscriberAgreement.supplierSharingAgreementShortName}
                            error={errors.SupplierSharingAgreementShortName}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>FTP User Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="FtpUserName"
                            name="FtpUserName"
                            placeholder="Ftp User Name"
                            required={true}
                            value={editSubscriberAgreement.ftpUserName}
                            error={errors.ftpUserName}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>FTP User Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="FtpUserName"
                            name="FtpUserName"
                            placeholder="Ftp User Name"
                            required={true}
                            value={editSubscriberAgreement.ftpUserName}
                            error={errors.ftpUserName}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>FTP Public Key</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="FtpPublicKey"
                            name="FtpPublicKey"
                            placeholder="Ftp Public Key"
                            required={true}
                            value={editSubscriberAgreement.ftpPublicKey}
                            error={errors.ftpPublicKey}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>GPG Public Key</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="FpgPublicKey"
                            name="GpgPublicKey"
                            placeholder="Gpg Public Key"
                            required={true}
                            value={editSubscriberAgreement.gpgPublicKey}
                            error={errors.gpgPublicKey}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Active</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="IsActive"
                            name="isActive"
                            label=""
                            checked={editSubscriberAgreement.isActive}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

        </SummaryListBase>

            <div>
                <ButtonBase onClick={() => handleCancel()} cancel>Cancel</ButtonBase>
                <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.edit}>
                    <ButtonBase onClick={handleUpdate} edit>Update</ButtonBase>
                </SecuredComponents>
            </div>
        </>
    );
}

export default SubscriberAgreementDetailCardEdit;
