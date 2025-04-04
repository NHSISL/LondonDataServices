import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import TextInputBase from "../bases/inputs/TextInputBase";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import { useValidation } from "../../hooks/useValidation";
import { DataTypeView } from "../../models/views/components/dataTypes/dataTypeView";
import { dataTypeErrors } from "./dataTypeErrors";
import { dataTypeValidations } from "./dataTypeValidations";

interface DataTypeRowEditProps {
    dataType: DataTypeView;
    onCancel: () => void;
    onEdit: (dataType: DataTypeView) => void;
    apiError?: any;
}

const DataTypeRowEdit: FunctionComponent<DataTypeRowEditProps> = (props) => {
    const {
        dataType,
        onCancel,
        onEdit,
        apiError
    } = props;

    const [editDataType, setEditDataType] = useState<DataTypeView>({ ...dataType });

    const { errors, processApiErrors, enableValidationMessages, validate } =
        useValidation(dataTypeErrors, dataTypeValidations, editDataType);

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) => {
        const updatedDataType = {
            ...editDataType,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setEditDataType(updatedDataType);
    };

    const handleCancel = () => {
        setEditDataType({ ...dataType });
        onCancel();
    };

    const handleUpdate = () => {
        if (!validate(editDataType)) {
            onEdit(editDataType);
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
                    label="Data Type Name"
                    placeholder="Data Type Name"
                    value={editDataType.name}
                    required={true}
                    error={errors.name}
                    onChange={handleChange} />
            </TableBaseData>
            
            <TableBaseData></TableBaseData>
            
            <TableBaseData classes="text-end">
                <ButtonBase onClick={() => handleCancel()} cancel>Cancel</ButtonBase>&nbsp;
                <ButtonBase onClick={() => handleUpdate()} disabled={errors.hasErrors} edit>Update</ButtonBase>
            </TableBaseData>
            
        </TableBaseRow>
    );
}

export default DataTypeRowEdit;
