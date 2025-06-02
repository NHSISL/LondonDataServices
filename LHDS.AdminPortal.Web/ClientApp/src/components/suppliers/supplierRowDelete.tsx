import React, { FunctionComponent } from "react";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { Guid } from "guid-typescript";
import { toastSuccess } from "../../brokers/toastBroker";

interface SupplierRowDeleteProps {
    supplier: SupplierView;
    onCancel: (id: Guid) => void;
    onDelete: (supplier: SupplierView) => void;
}

const SupplierRowDelete: FunctionComponent<SupplierRowDeleteProps> = (props) => {
    const {
        supplier,
        onCancel,
        onDelete
    } = props;

    const handleDelete = (supplier: SupplierView) => {
        onDelete(supplier);
        toastSuccess(`${supplier.name} Deleted`);
    }

    return (
        <TableBaseRow>
            <TableBaseData>
                <b>{supplier.name} - {supplier.friendlyName}</b><br />
                {supplier.description}<br />
            </TableBaseData>
            <TableBaseData classes="text-end">
                <ButtonBase onClick={() => onCancel(supplier.id)} cancel>Cancel</ButtonBase>&nbsp;
                <ButtonBase onClick={() => handleDelete(supplier)} remove>Yes, Delete</ButtonBase>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default SupplierRowDelete;

