import React, { FunctionComponent } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { SecuredComponents } from "../links";
import { DataTypeView } from "../../models/views/components/dataTypes/dataTypeView";

interface DataTypeRowViewProps {
    dataType: DataTypeView;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
    onEdit: (value: string) => void;
    onDelete: (value: string) => void;
}

const DataTypeRowView: FunctionComponent<DataTypeRowViewProps> = (props) => {
    const {
        dataType,
        allowedToEdit,
        allowedToDelete,
        onEdit,
        onDelete
    } = props;

    return (
        <TableBaseRow>
            <TableBaseData>
                {
                    dataType.name &&
                    (<><b>{dataType.name}</b></>)
                }
            </TableBaseData>
            <TableBaseData></TableBaseData>
            <TableBaseData classes="text-end">
                {allowedToEdit && (
                    <SecuredComponents>
                        <ButtonBase onClick={() => onEdit('EDIT')} edit>Edit</ButtonBase>
                    </SecuredComponents>
                )}
                &nbsp;
                {allowedToDelete && (
                    <SecuredComponents>
                        <ButtonBase onClick={() => onDelete('DELETE')} remove>Delete</ButtonBase>
                    </SecuredComponents>
                )}
            </TableBaseData>
        </TableBaseRow>
    );
}

export default DataTypeRowView;
