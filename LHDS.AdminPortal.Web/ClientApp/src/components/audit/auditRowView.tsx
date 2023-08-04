import React, { FunctionComponent } from "react";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import { AuditView } from "../../models/views/components/audit/auditView";
import moment from "moment";

interface AuditRowViewProps {
    audit: AuditView;
}

const AuditRowView: FunctionComponent<AuditRowViewProps> = (props) => {
    const {
        audit,
    } = props;

    return (
        <>
            <TableBaseRow>
                <TableBaseData>
                    {audit.message}
                </TableBaseData>
                <TableBaseData>
                    {moment(audit.createdDate?.toString()).format("Do-MMM-yyyy HH:mm:ss")}
                </TableBaseData>
            </TableBaseRow>
        </>
    );
}
export default AuditRowView;