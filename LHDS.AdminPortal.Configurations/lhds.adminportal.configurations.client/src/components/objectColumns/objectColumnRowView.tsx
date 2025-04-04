import React, { FunctionComponent } from "react";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import moment from "moment";
import { ObjectColumn } from "../../models/objectColumns/objectColumn";

type ObjectColumnRowProps = {
    objectColumn: ObjectColumn;
    dataSetSpecificationId: string;
}

const ObjectColumnRowView: FunctionComponent<ObjectColumnRowProps> = (props) => {
    const {
        objectColumn,
        dataSetSpecificationId
    } = props;

    return (
        <TableBaseRow>
            <TableBaseData>
                {objectColumn.ourColumnName}
            </TableBaseData>
            <TableBaseData>
                {objectColumn.supplierColumnName}
            </TableBaseData>
            <TableBaseData>
                {objectColumn.createdBy}
            </TableBaseData>
            <TableBaseData>
                {moment(objectColumn.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
            </TableBaseData>
            <TableBaseData>
                <Link to={'/configuration/ObjectColumn/' + dataSetSpecificationId + '/' + objectColumn?.id?.toString() + '/' + objectColumn.specificationObjectId.toString()}>
                    <ButtonBase onClick={() => { }} add>
                        View/Edit
                    </ButtonBase>
                </Link>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default ObjectColumnRowView;