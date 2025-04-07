import React, { FunctionComponent, useState } from "react";
import { DataTypeView } from "../../models/views/components/dataTypes/dataTypeView";
import DataTypeRowView from "./dataTypeRowView";
import DataTypeRowEdit from "./dataTypeRowEdit";
import DataTypeRowDelete from "./dataTypeRowDelete";
import { ApiError } from "../../types/apiError";


type DataTypeRowProps = {
    dataType: DataTypeView;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
    onUpdate: (dataType: DataTypeView) => void;
    onDelete: (dataType: DataTypeView) => void;
}

const DataTypeRow: FunctionComponent<DataTypeRowProps> = (props) => {
    const {
        dataType,
        allowedToEdit,
        allowedToDelete,
        onUpdate,
        onDelete,
    } = props;

    const [mode, setMode] = useState<string>('VIEW');
    const [apiError, setApiError] = useState<ApiError>({ response: { data: { errors: {} } } });

    const handleMode = (value: string) => {
        setMode(value);
    };

    const handleUpdate = async (dataType: DataTypeView) => {
        try {
            await onUpdate(dataType);
            setMode('VIEW');
        } catch (error) {
            setApiError(error);
            setMode('EDIT');
        }
    };

    const handleDelete = (dataType: DataTypeView) => {
        onDelete(dataType);
        setMode('VIEW');
    };

    const handleCancel = () => {
        setMode('VIEW');
    };

    return (
        <>
            {mode !== 'EDIT' && mode !== 'DELETE' && (
                <DataTypeRowView
                    key={dataType.id.toString()}
                    dataType={dataType}
                    onEdit={handleMode}
                    onDelete={handleMode}
                    allowedToEdit={allowedToEdit}
                    allowedToDelete={allowedToDelete} />
            )}

            {mode === 'EDIT' && (
                <DataTypeRowEdit
                    key={dataType.id.toString()}
                    dataType={dataType}
                    onCancel={handleCancel}
                    onEdit={handleUpdate}
                    apiError={apiError}
                />
            )}

            {mode === 'DELETE' && (
                <DataTypeRowDelete
                    key={dataType.id.toString()}
                    dataType={dataType}
                    onCancel={handleCancel}
                    onDelete={handleDelete} />
            )}
            
        </>
    );
}

DataTypeRow.defaultProps = {
    allowedToEdit: false,
    allowedToDelete: false
};

export default DataTypeRow;