import React, { FunctionComponent } from "react";
import { DataSet } from "../../models/dataSets/dataSet";
import DataSetRowView from "./dataSetRowView";

type DataSetRowProps = {
    dataSet: DataSet;
};

const DataSetRow: FunctionComponent<DataSetRowProps> = (props) => {
    const {
        dataSet
    } = props;

    return (
        <DataSetRowView
            key={dataSet.id.toString()}
            dataSet={dataSet} />
    );
};

export default DataSetRow;