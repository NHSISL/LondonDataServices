import React, { FunctionComponent } from "react";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import moment from "moment";
import { DataSet } from "../../models/dataSets/dataSet";

type DataSetRowProps = {
    dataSet: DataSet;
}

const DataSetRowView: FunctionComponent<DataSetRowProps> = (props) => {
    const {
        dataSet
    } = props;

    return (
        <TableBaseRow>
            <TableBaseData>
                {dataSet.dataSetName}
            </TableBaseData>
            <TableBaseData>
                {dataSet.createdBy}
            </TableBaseData>
            <TableBaseData>
                {moment(dataSet.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
            </TableBaseData>
            <TableBaseData>
                <Link to={'/configuration/dataSet/' + dataSet?.id?.toString()}>
                    <ButtonBase onClick={() => { }} add>
                        View/Edit
                    </ButtonBase>
                </Link>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default DataSetRowView;