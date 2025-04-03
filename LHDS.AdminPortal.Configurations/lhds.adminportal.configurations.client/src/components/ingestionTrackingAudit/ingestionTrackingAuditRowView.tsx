import React, { FunctionComponent } from "react";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import { IngestionTrackingAuditView } from "../../models/views/components/ingestionTrackingAudit/ingestionTrackingAuditView";
import moment from "moment";

interface IngestionTrackingAuditRowViewProps {
    ingestionTrackingAudit: IngestionTrackingAuditView;
}

const IngestionTrackingAuditRowView: FunctionComponent<IngestionTrackingAuditRowViewProps> = (props) => {
    const {
        ingestionTrackingAudit,
    } = props;

    return (
        <>
            <TableBaseRow>
                <TableBaseData>
                    {ingestionTrackingAudit.message}
                </TableBaseData>
                <TableBaseData>
                    {moment(ingestionTrackingAudit.createdDate?.toString()).format("Do-MMM-yyyy HH:mm:ss")}
                </TableBaseData>
            </TableBaseRow>
        </>
    );
}

export default IngestionTrackingAuditRowView;