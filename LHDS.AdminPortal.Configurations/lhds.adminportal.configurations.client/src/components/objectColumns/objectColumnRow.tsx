import React, { FunctionComponent } from "react";
import { ObjectColumn } from "../../models/objectColumns/objectColumn";
import ObjectColumnRowView from "./objectColumnRowView";

type ObjectColumnRowProps = {
    objectColumn: ObjectColumn;
    dataSetSpecificationId: string;
};

const ObjectColumnRow: FunctionComponent<ObjectColumnRowProps> = (props) => {
    const {
        objectColumn,
        dataSetSpecificationId
    } = props;

    return (
        <ObjectColumnRowView
            key={objectColumn.id.toString()}
            objectColumn={objectColumn}
            dataSetSpecificationId={dataSetSpecificationId}
        />
    );
};

export default ObjectColumnRow;