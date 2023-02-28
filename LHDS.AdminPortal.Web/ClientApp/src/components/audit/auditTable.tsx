import { Guid } from "guid-typescript";
import React, { FunctionComponent } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";
import AuditRow from "./auditRow";

interface AuditTableProps {
    ingestionTrackingId: string
}

const AuditTable: FunctionComponent<AuditTableProps> = (props) => {
    const {
        ingestionTrackingId
    } = props;

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Audit
                    </CardBaseTitle>
                    <CardBaseContent>
                        <TableBase>
                            <TableBaseTbody>
                                <AuditRow ingestionTrackingId={ingestionTrackingId} />
                            </TableBaseTbody>
                        </TableBase>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div >
    );
}
export default AuditTable;