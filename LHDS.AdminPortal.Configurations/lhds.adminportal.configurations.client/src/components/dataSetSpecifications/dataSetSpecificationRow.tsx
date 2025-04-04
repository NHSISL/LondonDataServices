import React, { FunctionComponent } from "react";
import { DataSetSpecification } from "../../models/dataSetSpecifications/dataSetSpecification";
import DataSetSpecificationRowView from "./dataSetSpecificationRowView";

type DataSetSpecificationRowProps = {
    dataSetSpecification: DataSetSpecification;
};

const DataSetSpecificationRow: FunctionComponent<DataSetSpecificationRowProps> = (props) => {
    const {
        dataSetSpecification
    } = props;

    return (
        <DataSetSpecificationRowView
            key={dataSetSpecification.id.toString()}
            dataSetSpecification={dataSetSpecification} />
    );
};

export default DataSetSpecificationRow;