import React from "react";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";

const AuditRowView = () => {
    return (
        <div>
            <TableBaseRow>
                <TableBaseData>
                    TEST DATA
                </TableBaseData>
                <TableBaseData>
                    23-Aug-2022
                </TableBaseData>
            </TableBaseRow>
            <TableBaseRow>
                <TableBaseData>
                    TEST DATA
                </TableBaseData>
                <TableBaseData>
                    TEST DATE
                </TableBaseData>
            </TableBaseRow>
           
        </div>
    );
}
export default AuditRowView;