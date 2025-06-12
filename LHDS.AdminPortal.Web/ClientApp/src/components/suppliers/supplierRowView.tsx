import React, { FunctionComponent } from "react";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { SecuredComponents } from "../links";

interface SupplierRowViewProps {
    supplier: SupplierView;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
    onEdit: (value: string) => void;
    onDelete: (value: string) => void;
}

const SupplierRowView: FunctionComponent<SupplierRowViewProps> = (props) => {
    const {
        supplier,
        allowedToEdit,
        allowedToDelete,
        onEdit,
        onDelete
    } = props;

    return (
        <TableBaseRow>
            <TableBaseData>
                {
                    supplier.name &&
                    (<><b>{supplier.name}</b> - {supplier.friendlyName}</>)
                }
                {
                    supplier.description &&
                    <><br/>{supplier.description}</>
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

export default SupplierRowView;
