import React, { FunctionComponent, useState } from "react";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import SupplierRowView from "./supplierRowView";
import SupplierRowEdit from "./supplierRowEdit";
import SupplierRowDelete from "./supplierRowDelete";

type SupplierRowProps = {
    supplier: SupplierView;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
    onUpdate: (supplier: SupplierView) => void;
    onDelete: (supplier: SupplierView) => void;
}

const SupplierRow: FunctionComponent<SupplierRowProps> = (props) => {
    const {
        supplier,
        allowedToEdit,
        allowedToDelete,
        onUpdate,
        onDelete,
    } = props;

    const [mode, setMode] = useState<string>('VIEW');
    const [apiError, setApiError] = useState<any>({});


    const handleMode = (value: string) => {
        setMode(value);
    };

    const handleUpdate = async (supplier: SupplierView) => {
        try {
            await onUpdate(supplier);
            setMode('VIEW');
        } catch (error) {
            setApiError(error);
            setMode('EDIT');
        }
    };

    const handleDelete = (supplier: SupplierView) => {
        onDelete(supplier);
        setMode('VIEW');
    };

    const handleCancel = () => {
        setMode('VIEW');
    };

    return (
        <>
            {mode !== 'EDIT' && mode !== 'DELETE' && (
                <SupplierRowView
                    key={supplier.id.toString()}
                    supplier={supplier}
                    onEdit={handleMode}
                    onDelete={handleMode}
                    allowedToEdit={allowedToEdit}
                    allowedToDelete={allowedToDelete} />
            )}

            {mode === 'EDIT' && (
                <SupplierRowEdit
                    key={supplier.id.toString()}
                    supplier={supplier}
                    onCancel={handleCancel}
                    onEdit={handleUpdate}
                    apiError={apiError}
                />
            )}

            {mode === 'DELETE' && (
                <SupplierRowDelete
                    key={supplier.id.toString()}
                    supplier={supplier}
                    onCancel={handleCancel}
                    onDelete={handleDelete} />
            )}
        </>
    );
}

SupplierRow.defaultProps = {
    allowedToEdit: false,
    allowedToDelete: false
};

export default SupplierRow;