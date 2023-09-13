import React, { FunctionComponent } from "react";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import moment from "moment";
import { SpecificationObject } from "../../models/specificationObjects/specificationObject";

type SpecificationObjectRowProps = {
    specificationObject: SpecificationObject;
}

const SpecificationObjectRowView: FunctionComponent<SpecificationObjectRowProps> = (props) => {
    const {
        specificationObject
    } = props;

    return (
        <TableBaseRow>
            <TableBaseData>
                {specificationObject.ourObjectName}
            </TableBaseData>
            <TableBaseData>
                {specificationObject.supplierObjectName}
            </TableBaseData>
            <TableBaseData>
                {specificationObject.createdBy}
            </TableBaseData>
            <TableBaseData>
                {moment(specificationObject.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
            </TableBaseData>
            <TableBaseData>
                <Link to={'/configuration/SpecificationObject/' + specificationObject?.id?.toString() + '/' + specificationObject.dataSetSpecificationId.toString()}>
                    <ButtonBase onClick={() => { }} add>
                        View/Edit
                    </ButtonBase>
                </Link>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default SpecificationObjectRowView;