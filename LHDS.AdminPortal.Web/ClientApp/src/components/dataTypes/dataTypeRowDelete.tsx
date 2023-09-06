import React, { FunctionComponent } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { Guid } from "guid-typescript";
import { toastSuccess } from "../../brokers/toastBroker";
import { DataTypeView } from "../../models/views/components/dataTypes/dataTypeView";

interface DataTypeRowDeleteProps {
    dataType: DataTypeView;
    onCancel: (id: Guid) => void;
    onDelete: (dataType: DataTypeView) => void;
}

const DataTypeRowDelete: FunctionComponent<DataTypeRowDeleteProps> = (props) => {
    const {
        dataType,
        onCancel,
        onDelete
    } = props;

    const handleDelete = (dataType: DataTypeView) => {
        onDelete(dataType);
        toastSuccess(`${dataType.name} Deleted`);
    }

    return (
        <TableBaseRow>
            <TableBaseData>
                {dataType.name}
            </TableBaseData>
            <TableBaseData></TableBaseData>
            <TableBaseData classes="text-end">
                <ButtonBase onClick={() => onCancel(dataType.id)} cancel>Cancel</ButtonBase>&nbsp;
                <ButtonBase onClick={() => handleDelete(dataType)} remove>Yes, Delete</ButtonBase>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default DataTypeRowDelete;

