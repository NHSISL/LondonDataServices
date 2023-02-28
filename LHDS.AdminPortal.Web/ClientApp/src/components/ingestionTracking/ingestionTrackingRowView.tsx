import React, { FunctionComponent } from "react";
import { Badge } from "react-bootstrap";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";

type IngestionTrackingRowProps = {
    ingestionTracking: IngestionTracking;
}

const IngestionTrackingRow: FunctionComponent<IngestionTrackingRowProps> = (props) => {
    const {
        ingestionTracking
    } = props;

    return (
        <TableBaseRow>
            <TableBaseData>
                {ingestionTracking.source}
            </TableBaseData>
            <TableBaseData>
                {ingestionTracking.encryptedFileName}
                <br />
                {ingestionTracking.decrypted && <Badge pill bg="success text-white">Re-land</Badge>}
                {ingestionTracking.decrypted && <Badge pill bg="success text-white">Decrypt</Badge>}
                <Badge pill bg="success text-white">Download</Badge> &nbsp;
                <Badge pill bg="success text-white"><Link to={`/ingestionTrackingDetail/${ingestionTracking.id}`}>Details</Link></Badge>
            </TableBaseData>
            <TableBaseData>
                <Link to={`/ingestionTrackingDetail/${ingestionTracking.id}`}>
                    <ButtonBase onClick={() => { }} add>
                        Details
                    </ButtonBase>
                </Link>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default IngestionTrackingRow;
