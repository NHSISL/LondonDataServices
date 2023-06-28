import React, { FunctionComponent } from "react";
import TableBaseRow from "../../bases/components/Table/TableBase.Row";
import TableBaseData from "../../bases/components/Table/TableBase.Data";
import { Pds } from "../../../models/pds/pds";
import moment from "moment";

type PdsRowProps = {
    pds: Pds;
}

const PdsRow: FunctionComponent<PdsRowProps> = (props) => {
    const {
        pds
    } = props;

    return (<>

        <TableBaseRow>
            <TableBaseData>
                {pds.message}
            </TableBaseData>
            <TableBaseData>
                {pds.fileName}
            </TableBaseData>
            <TableBaseData>
                {pds.messageId}
            </TableBaseData>
            <TableBaseData>
                <small>Created By: {pds.createdBy}</small>
                <br/>
                <small>Created Date: {moment(pds.createdDate?.toString()).format("Do-MMM-yyyy HH:mm:ss")}</small>
            </TableBaseData>
        </TableBaseRow>
    </>
    );
}

export default PdsRow;