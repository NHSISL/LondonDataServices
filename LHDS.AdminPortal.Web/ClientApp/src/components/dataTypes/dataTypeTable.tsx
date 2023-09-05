import React, { FunctionComponent} from "react";

type DataTypeTableProps = {
    allowedToAdd: boolean;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
}

const DataTypeTable: FunctionComponent<DataTypeTableProps> = (props) => {
    const {
        allowedToAdd,
        allowedToEdit,
        allowedToDelete,
    } = props;

    return (
        <>
            <h1>TODO: Data Types Table Screen</h1>
            {allowedToAdd}{allowedToEdit}{allowedToDelete}
        </>
    );
}

export default DataTypeTable;