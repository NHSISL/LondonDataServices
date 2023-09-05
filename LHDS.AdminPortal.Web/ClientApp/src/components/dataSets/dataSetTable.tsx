import React, { FunctionComponent} from "react";

type DataTypeTableProps = {
    allowedToAdd: boolean;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
}

const DataSetTable: FunctionComponent<DataTypeTableProps> = (props) => {
    const {
        allowedToAdd,
        allowedToEdit,
        allowedToDelete,
    } = props;

    return (
        <>
           {allowedToAdd}{allowedToEdit}{allowedToDelete}
           <h1>TODO: Data Sets Table Screen</h1>
        </>
    );
}

export default DataSetTable;