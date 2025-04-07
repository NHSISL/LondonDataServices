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
import CheckboxBase from "../bases/inputs/CheckboxBase";
import { Link } from "react-router-dom";
import { SubscriberCredentialView } from "../../models/views/components/subscriberCredentials/subscriberCredentialView";
import { subscriberAgreementErrors } from "./subscriberAgreementErrors";
import { subscriberAgreementValidation } from "./subscriberAgreementValidation";
import { ApiError } from "../../types/apiError";

interface SubscriberAgreementDetailCardEditProps {
    subscriberCredential: SubscriberCredentialView;
    onAdd: (subscriberCredential: SubscriberCredentialView) => void;
    onUpdate: (subscriberCredential: SubscriberCredentialView) => void;
    onCancel: () => void;
    mode: string;
    onModeChange: (value: string) => void;
    apiError?: ApiError;
}

const SubscriberAgreementDetailCardEdit: FunctionComponent<SubscriberAgreementDetailCardEditProps> = (props) => {
    const {
        subscriberCredential,
        onAdd,
        onUpdate,
        onCancel,
        mode,
        onModeChange,
        apiError
    } = props;

    const [editSubscriberCredential, setEditSubscriberCredential] = useState<SubscriberCredentialView>({ ...subscriberCredential });

    const { errors, enableValidationMessages, processApiErrors, validate } = useValidation(subscriberAgreementErrors, subscriberAgreementValidation, editSubscriberCredential)

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement> | ChangeEvent<HTMLSelectElement>) => {
        console.log("Handling change...");
        const updatedSubscriberCredential = {
            ...editSubscriberCredential,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked
                : event.target.value,
        };

        console.log("Updated Credential:", updatedSubscriberCredential);
        setEditSubscriberCredential(updatedSubscriberCredential);
    };

    const handleCancel = () => {
        setEditSubscriberCredential({ ...subscriberCredential });
        onModeChange('VIEW')
        onCancel();
    };

    const handleSave = () => {
        if (!validate(editSubscriberCredential)) {
            onAdd(editSubscriberCredential)
        } else {
            enableValidationMessages();
        }
    }

    const handleUpdate = () => {
        if (!validate(editSubscriberCredential)) {
            onUpdate(editSubscriberCredential)
        } else {
            enableValidationMessages();
        }
    }

    useEffect(() => {
        processApiErrors(apiError)
    }, [apiError, processApiErrors])

    useEffect(() => {
        setEditSubscriberCredential({ ...subscriberCredential });
    }, [subscriberCredential]);

    return (
        <>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Short Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="supplierSharingAgreementShortName"
                            name="supplierSharingAgreementShortName"
                            placeholder="Supplier Sharing Agreement ShortName"
                            value={editSubscriberCredential.supplierSharingAgreementShortName}
                            error={errors.SupplierSharingAgreementShortName}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Sharing Agreement Guid</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="supplierSharingAgreementGuid"
                            name="supplierSharingAgreementGuid"
                            placeholder="Supplier Sharing Agreement Guid"
                            value={editSubscriberCredential.supplierSharingAgreementGuid!.toString()}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>FTP User Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="ftpUserName"
                            name="ftpUserName"
                            placeholder="Ftp User Name"
                            value={editSubscriberCredential.ftpUserName}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>FTP Public Key</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {editSubscriberCredential.ftpPublicKey}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>GPG Public Key</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {editSubscriberCredential.gpgPublicKey}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Active</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isActive"
                            name="isActive"
                            label=""
                            checked={editSubscriberCredential.isActive}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

            </SummaryListBase>

            {mode === "ADD" && (
                <div>
                    <Link to={'/configuration/subscriberAgreements/'}>
                        <ButtonBase onClick={() => { }} add>Cancel</ButtonBase>
                    </Link>
                    <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.add}>
                        <ButtonBase onClick={handleSave} add>Add</ButtonBase>
                    </SecuredComponents>
                </div>
            )}

            {mode !== "ADD" && (
                <div>
                    <ButtonBase onClick={() => handleCancel()} cancel>Cancel</ButtonBase>
                    <SecuredComponents allowedRoles={securityPoints.subscriberAgreement.edit}>
                        <ButtonBase onClick={handleUpdate} edit>Update</ButtonBase>
                    </SecuredComponents>
                </div>
            )}
        </>
    );
}

export default SubscriberAgreementDetailCardEdit;
