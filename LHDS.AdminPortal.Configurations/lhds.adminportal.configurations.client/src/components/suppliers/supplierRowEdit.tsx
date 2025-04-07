import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import TextInputBase from "../bases/inputs/TextInputBase";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import { useValidation } from "../../hooks/useValidation";
import { supplierErrors } from "./supplierErrors";
import { supplierValidations } from "./supplierValidations";
import TextAreaInputBase from "../bases/inputs/TextAreaInputBase";
import CheckboxBase from "../bases/inputs/CheckboxBase";
import { ApiError } from "../../types/apiError";

interface SupplierRowEditProps {
    supplier: SupplierView;
    onCancel: () => void;
    onEdit: (supplier: SupplierView) => void;
    apiError?: ApiError;
}

const SupplierRowEdit: FunctionComponent<SupplierRowEditProps> = (props) => {
    const {
        supplier,
        onCancel,
        onEdit,
        apiError
    } = props;

    const [editSupplier, setEditSupplier] = useState<SupplierView>({ ...supplier });

    const { errors, processApiErrors, enableValidationMessages, validate } =
        useValidation(supplierErrors, supplierValidations, editSupplier);

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) => {
        const updatedSupplier = {
            ...editSupplier,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setEditSupplier(updatedSupplier);
    };

    const handleCancel = () => {
        setEditSupplier({ ...supplier });
        onCancel();
    };

    const handleUpdate = () => {
        if (!validate(editSupplier)) {
            onEdit(editSupplier);
        } else {
            enableValidationMessages();
        }
    }

    useEffect(() => {
        processApiErrors(apiError);
    }, [apiError, processApiErrors])

    return (
        <TableBaseRow>
            <TableBaseData>

                <TextInputBase
                    id="name"
                    name="name"
                    label="Supplier Name"
                    placeholder="Supplier Name"
                    value={editSupplier.name}
                    required={true}
                    error={errors.name}
                    onChange={handleChange} />

                <br />

                <TextInputBase
                    id="friendlyName"
                    name="friendlyName"
                    label="Friendly Name"
                    placeholder="Friendly Name"
                    value={editSupplier.friendlyName}
                    required={true}
                    error={errors.name}
                    onChange={handleChange} />

                <br />

                <TextAreaInputBase
                    id="description"
                    name="description"
                    label="Supplier Description"
                    placeholder="Supplier Description"
                    value={editSupplier.description}
                    required={true}
                    error={errors.description}
                    onChange={handleChange}
                    rows={3} />

                <br />

                <TextInputBase
                    id="landingManualTriggerUrl"
                    name="landingManualTriggerUrl"
                    label="landing Manual Trigger Url"
                    placeholder="landing Manual Trigger Url"
                    value={editSupplier.landingManualTriggerUrl}
                    required={true}
                    error={errors.name}
                    onChange={handleChange} />

            </TableBaseData>

            <TableBaseData>

                <TextInputBase
                    id="decryptionManualTriggerUrl"
                    name="decryptionManualTriggerUrl"
                    label="decryption Manual Trigger Url"
                    placeholder="Decryption Manual Trigger Url"
                    value={editSupplier.landingManualTriggerUrl}
                    required={true}
                    error={errors.name}
                    onChange={handleChange} />

                <br />

                <CheckboxBase
                    id="canDecryptIngestionTracking"
                    name="canDecryptIngestionTracking"
                    label="Can Decrypt Ingestion Tracking"
                    checked={editSupplier.canDecryptIngestionTracking === true ? true : false}
                    error={errors.canDecryptIngestionTracking}
                    onChange={handleChange} />

                <br />

                <CheckboxBase
                    id="canDownloadIngestionTracking"
                    name="canDownloadIngestionTracking"
                    label="Can Download Ingestion Tracking"
                    checked={editSupplier.canDownloadIngestionTracking === true ? true : false}
                    error={errors.canDownloadIngestionTracking}
                    onChange={handleChange} />

                <br />

                <CheckboxBase
                    id="canRelandIngestionTracking"
                    name="canRelandIngestionTracking"
                    label="Can Reland Ingestion Tracking"
                    checked={editSupplier.canRelandIngestionTracking === true ? true : false}
                    error={errors.canRelandIngestionTracking}
                    onChange={handleChange} />

            </TableBaseData>

            <TableBaseData classes="text-end">
                <ButtonBase onClick={() => handleCancel()} cancel>Cancel</ButtonBase>&nbsp;
                <ButtonBase onClick={() => handleUpdate()} disabled={errors.hasErrors} edit>Update</ButtonBase>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default SupplierRowEdit;