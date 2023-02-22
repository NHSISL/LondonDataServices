import React from "react";
import { Badge } from "react-bootstrap";
import { Link } from "react-router-dom";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";

const AuditRowView = () => {

    return (
        <div>
            <TableBaseRow>
                <TableBaseData>
                    Landed document - /emisnightingale-data-preprod-provider-extracts/IM1/sftp/70CD5674-1F0E-44E8-95C5-75D70EA9A291/20230113/delta_76606_Admin_Location_20230113134411_70CD5674-1F0E-44E8-95C5-75D70EA9A291.csv.gpg
                </TableBaseData>
                <TableBaseData>
                    23-Aug-2022
                </TableBaseData>
            </TableBaseRow>
            <TableBaseRow>
                <TableBaseData>
                    Decrypted document - /encrypted/emisnightingale-data-preprod-provider-extracts/IM1/sftp/70CD5674-1F0E-44E8-95C5-75D70EA9A291/20230109/delta_76356_Appointment_Slot_20230109132842_70CD5674-1F0E-44E8-95C5-75D70EA9A291.csv
                </TableBaseData>
                <TableBaseData>
                    23-Aug-2022
                </TableBaseData>
            </TableBaseRow>
           
        </div>
    );
}
export default AuditRowView;