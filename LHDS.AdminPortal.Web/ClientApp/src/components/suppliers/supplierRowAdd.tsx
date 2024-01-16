import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import { Guid } from "guid-typescript";
import TextInputBase from "../bases/inputs/TextInputBase";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { useValidation } from "../../hooks/useValidation";
import { supplierValidations } from "./supplierValidations";
import { supplierErrors } from "./supplierErrors";
import TextAreaInputBase from "../bases/inputs/TextAreaInputBase";

interface SupplierRowAddProps {
    onCancel: () => void;
    onAdd: (supplier: SupplierView) => void;
    apiError?: any;
}

const SupplierRowAdd: FunctionComponent<SupplierRowAddProps> = (props) => {
    const {
        onCancel,
        onAdd,
        apiError
    } = props;

    const [supplier, setSupplier] = useState<SupplierView>(new SupplierView(Guid.create()));

    const { errors, processApiErrors, enableValidationMessages, validate } =
        useValidation(supplierErrors, supplierValidations, supplier);

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) => {
        const addSupplier = {
            ...supplier,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setSupplier(addSupplier);
    };

    const handleSave = () => {
        if (!validate(supplier)) {
            onAdd(supplier);
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
                    value={supplier.name}
                    required={true}
                    error={errors.name}
                    onChange={handleChange} />
                <TextAreaInputBase
                    id="description"
                    name="description"
                    label="Supplier Description"
                    placeholder="Supplier Description"
                    value={supplier.description}
                    error={errors.description}
                    onChange={handleChange}
                    rows={3}
                />
            </TableBaseData>

            <TableBaseData>
                <TextInputBase
                    id="friendlyName"
                    name="friendlyName"
                    label="Friendly Name"
                    placeholder="Friendly Name"
                    value={supplier.friendlyName}
                    required={true}
                    error={errors.friendlyName}
                    onChange={handleChange} />
                <TextInputBase
                    id="landingManualTriggerUrl"
                    name="landingManualTriggerUrl"
                    label="landing Manual Trigger Url"
                    placeholder="landing Manual Trigge Url"
                    value={supplier.landingManualTriggerUrl}
                    required={true}
                    error={errors.landingManualTriggerUrl}
                    onChange={handleChange} />
                <TextInputBase
                    id="decryptionManualTriggerUrl"
                    name="decryptionManualTriggerUrl"
                    label="Decryption Manual Trigger Url"
                    placeholder="landing Manual Trigge Url"
                    value={supplier.decryptionManualTriggerUrl}
                    required={true}
                    error={errors.landingManualTriggerUrl}
                    onChange={handleChange} />
            </TableBaseData>
            
            <TableBaseData classes="text-end">
                <ButtonBase onClick={() => onCancel()} cancel>Cancel</ButtonBase>&nbsp;
                <ButtonBase onClick={() => handleSave()} disabled={errors.hasErrors} add>Add</ButtonBase>
            </TableBaseData>

        </TableBaseRow>
    );
}

export default SupplierRowAdd;
