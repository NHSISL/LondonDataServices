import React, { FunctionComponent } from "react";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import moment from "moment";
import { DataSetSpecification } from "../../models/dataSetSpecifications/dataSetSpecification";

type DataSetSpecificationRowProps = {
    dataSetSpecification: DataSetSpecification;
}

const DataSetSpecificationRowView: FunctionComponent<DataSetSpecificationRowProps> = (props) => {
    const {
        dataSetSpecification
    } = props;

    return (
        <TableBaseRow>
            <TableBaseData >
                {dataSetSpecification.dataSet?.dataSetName}
            </TableBaseData>
            <TableBaseData classes="text-center">
                {dataSetSpecification.ourSpecificationVersion}
            </TableBaseData>
            <TableBaseData classes="text-center">
                {dataSetSpecification.supplierSpecificationVersion}
            </TableBaseData>
            <TableBaseData>
                {dataSetSpecification.createdBy}
            </TableBaseData>
            <TableBaseData>
                {moment(dataSetSpecification.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
            </TableBaseData>
            <TableBaseData>
                <Link to={'/configuration/dataSetSpecification/' + dataSetSpecification.dataSetId + '/' + dataSetSpecification?.id?.toString()}>
                    <ButtonBase onClick={() => { }} add>
                        View/Edit
                    </ButtonBase>
                </Link>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default DataSetSpecificationRowView;