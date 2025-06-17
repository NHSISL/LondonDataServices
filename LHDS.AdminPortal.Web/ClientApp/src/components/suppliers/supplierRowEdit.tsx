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

interface SupplierRowEditProps {
    supplier: SupplierView;
    onCancel: () => void;
    onEdit: (supplier: SupplierView) => void;
    apiError?: any;
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

            </TableBaseData>

            <TableBaseData>

                <CheckboxBase
                    id="isIngestionTracked"
                    name="isIngestionTracked"
                    label="is Ingestion Tracked"
                    checked={editSupplier.isIngestionTracked === true ? true : false}
                    error={errors.isIngestionTracked}
                    onChange={handleChange} />

                <br />

                <CheckboxBase
                    id="canDecryptIngestionTracking"
                    name="canDecryptIngestionTracking"
                    label="Can Decrypt Ingestion Tracking"
                    checked={editSupplier.canDecryptIngestionTracking === true ? true : false}
                    error={errors.canDecryptIngestionTracking}
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