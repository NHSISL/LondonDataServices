import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import TextInputBase from "../bases/inputs/TextInputBase";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { useValidation } from "../../hooks/useValidation";
import { DataTypeView } from "../../models/views/components/dataTypes/dataTypeView";
import { dataTypeErrors } from "./dataTypeErrors";
import { dataTypeValidations } from "./dataTypeValidations";
import { ApiError } from "../../types/apiError";

interface DataTypeRowAddProps {
    onCancel: () => void;
    onAdd: (dataType: DataTypeView) => void;
    apiError?: ApiError;
}

const DataTypeRowAdd: FunctionComponent<DataTypeRowAddProps> = (props) => {
    const {
        onCancel,
        onAdd,
        apiError
    } = props;

    const [dataType, setDataType] = useState<DataTypeView>(new DataTypeView(crypto.randomUUID,""));

    const { errors, processApiErrors, enableValidationMessages, validate } =
        useValidation(dataTypeErrors, dataTypeValidations, dataType);

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) => {
        const addDataType = {
            ...dataType,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setDataType(addDataType);
    };

    const handleSave = () => {
        if (!validate(dataType)) {
            onAdd(dataType);
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
                    value={dataType.name}
                    required={true}
                    error={errors.name}
                    onChange={handleChange} />
            </TableBaseData>

            <TableBaseData></TableBaseData>
            
            <TableBaseData classes="text-end">
                <ButtonBase onClick={() => onCancel()} cancel>Cancel</ButtonBase>&nbsp;
                <ButtonBase onClick={() => handleSave()} disabled={errors.hasErrors} add>Add</ButtonBase>
            </TableBaseData>
            
        </TableBaseRow>
    );
}

export default DataTypeRowAdd;
