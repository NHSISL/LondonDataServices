import React, { FunctionComponent } from "react";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";
import IngestionTrackingAuditRow from "./ingestionTrackingAuditRow";
import { ingestionTrackingAuditViewService } from "../../services/views/ingestionTrackingAudits/ingestionTrackingAuditViewService";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseThead from "../bases/components/Table/TableBase.Thead";
import { IngestionTrackingAuditView } from "../../models/views/components/ingestionTrackingAudit/ingestionTrackingAuditView";

interface IngestionTrackingAuditTableProps {
    ingestionTrackingId: string;
}

const IngestionTrackingAuditTable: FunctionComponent<IngestionTrackingAuditTableProps> = (props) => {
    const {
        ingestionTrackingId
    } = props;

    const { mappedAudits: auditsRetrieved } =
        ingestionTrackingAuditViewService.useGetAllAudits(ingestionTrackingId);

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
                    {auditsRetrieved?.map((ingestionTrackingAudit: IngestionTrackingAuditView) =>

                        <IngestionTrackingAuditRow
                            key={ingestionTrackingAudit.id?.toString()}
                            ingestionTrackingAudit={ingestionTrackingAudit}
                        />)}

                </TableBaseTbody>
            </TableBase>
        </div>
    );
}

export default IngestionTrackingAuditTable;