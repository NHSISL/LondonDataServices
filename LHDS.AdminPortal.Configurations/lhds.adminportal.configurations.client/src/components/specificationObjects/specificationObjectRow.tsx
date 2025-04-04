import React, { FunctionComponent } from "react";
import { SpecificationObject } from "../../models/specificationObjects/specificationObject";
import SpecificationObjectRowView from "./specificationObjectRowView";

type DataSetSpecificationRowProps = {
    specificationObject: SpecificationObject;
};

const SpecificationObjectRow: FunctionComponent<DataSetSpecificationRowProps> = (props) => {
    const {
        specificationObject
    } = props;

    return (
        <SpecificationObjectRowView
            key={specificationObject.id.toString()}
            specificationObject={specificationObject} />
    );
};

export default SpecificationObjectRow;