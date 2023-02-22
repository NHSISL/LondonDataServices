import { Table } from "nhsuk-react-components";
import React from "react";
import { Badge } from "react-bootstrap";
import { Link } from "react-router-dom";
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseRow from "../bases/components/Table/TableBase.Row";

const IngestionTrackingTable = () => {

    return (
        <div>
            <TableBaseRow>
                <TableBaseData>
                    EMIS
                </TableBaseData>
                <TableBaseData>
                    .../delta_76356_Admin_Location_20230109132842_70CD5674-1F0E-44E8-95C5-75D70EA9A291.csv.gpg
                    <br />
                    <Badge pill bg="success text-white">re-land</Badge> &nbsp;
                    <Badge pill bg="success text-white">decrypt</Badge> &nbsp;
                    <Badge pill bg="success text-white">download</Badge> &nbsp;
                </TableBaseData>
                <TableBaseData>
                    <Link to={'/ingestionTrackingDetail/'}>
                        <ButtonBase onClick={() => { }} add>
                            Details
                        </ButtonBase>
                    </Link>
                </TableBaseData>
            </TableBaseRow>

            <TableBaseRow>
                <TableBaseData>
                    EMIS
                </TableBaseData>
                <TableBaseData>
                    .../delta_76356_Admin_Location_20230109132842_70CD5674-1F0E-44E8-95C5-75D70EA9A291.csv.gpg
                    <br />
                    <Badge pill bg="success text-white">re-land</Badge> &nbsp;
                    <Badge pill bg="success text-white">decrypt</Badge> &nbsp;
                    <Badge pill bg="success text-white">download</Badge> &nbsp;
                </TableBaseData>
                <TableBaseData>
                    <Link to={'/ingestionTrackingDetail/'}>
                        <ButtonBase onClick={() => { }} add>
                            Details
                        </ButtonBase>
                    </Link>
                </TableBaseData>
            </TableBaseRow>
        </div>
    );
}
export default IngestionTrackingTable;