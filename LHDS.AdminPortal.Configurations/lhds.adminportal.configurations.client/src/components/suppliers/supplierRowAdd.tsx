import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import TextInputBase from "../bases/inputs/TextInputBase";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { useValidation } from "../../hooks/useValidation";
import { supplierValidations } from "./supplierValidations";
import { supplierErrors } from "./supplierErrors";
import TextAreaInputBase from "../bases/inputs/TextAreaInputBase";
import CheckboxBase from "../bases/inputs/CheckboxBase";

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

    const [supplier, setSupplier] = useState<SupplierView>(new SupplierView(crypto.randomUUID()));

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

                <CheckboxBase
                    id="isIngestionTracked"
                    name="isIngestionTracked"
                    label="is Ingestion Tracked"
                    checked={supplier.isIngestionTracked === true ? true : false}
                    error={errors.isIngestionTracked}
                    onChange={handleChange} />

                <CheckboxBase
                    id="canDecryptIngestionTracking"
                    name="canDecryptIngestionTracking"
                    label="Can Decrypt Ingestion Tracking"
                    checked={supplier.canDecryptIngestionTracking === true ? true : false}
                    error={errors.canDecryptIngestionTracking}
                    onChange={handleChange} />

                <br />
            </TableBaseData>


            
            <TableBaseData classes="text-end">
                <ButtonBase onClick={() => onCancel()} cancel>Cancel</ButtonBase>&nbsp;
                <ButtonBase onClick={() => handleSave()} disabled={errors.hasErrors} add>Add</ButtonBase>
            </TableBaseData>

        </TableBaseRow>
    );
}

export default SupplierRowAdd;
