import React, { FunctionComponent } from "react";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";
import AuditRow from "./auditRow";
import { auditViewService } from "../../services/views/auditViewService";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseThead from "../bases/components/Table/TableBase.Thead";
import { AuditView } from "../../models/views/components/audit/auditView";

interface AuditTableProps {
    ingestionTrackingId: string;
}

const AuditTable: FunctionComponent<AuditTableProps> = (props) => {
    const {
        ingestionTrackingId
    } = props;

    const { mappedAudits: auditsRetrieved } =
        auditViewService.useGetAllAudits(ingestionTrackingId);

    return (
        <div>
            <TableBase>
                <TableBaseThead>
                    <TableBaseRow>
                        <TableBaseData>Audit</TableBaseData>
                        <TableBaseData>Created Date</TableBaseData>
                    </TableBaseRow>
                </TableBaseThead>
                <TableBaseTbody>
                    {auditsRetrieved?.map((audit: AuditView) =>

                        <AuditRow
                            key={audit.id?.toString()}
                            audit={audit}
                        />)}

                </TableBaseTbody>
            </TableBase>
        </div>
    );
}
export default AuditTable;