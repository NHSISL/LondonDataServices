import React, { FunctionComponent } from "react";
import { Badge } from "react-bootstrap";
import TableBaseRow from "../../bases/components/Table/TableBase.Row";
import TableBaseData from "../../bases/components/Table/TableBase.Data";
import ButtonBase from "../../bases/buttons/ButtonBase";
import { Pds } from "../../../models/pds/pds";
import moment from "moment";
import TableBaseHeader from "../../bases/components/Table/TableBase.Header";

type PdsRowProps = {
    pds: Pds;
}

const PdsRow: FunctionComponent<PdsRowProps> = (props) => {
    const {
        pds
    } = props;

    function trimString(fileName: string) {

    }

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