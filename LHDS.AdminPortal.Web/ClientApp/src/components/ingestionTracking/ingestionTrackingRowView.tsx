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

    function trimString(fileName: string) {
        const str = fileName;
        const lastIndex = str.lastIndexOf('/');
        return str.substring(lastIndex + 1, str.length - 4);
    }

    return (
        <TableBaseRow>
            <TableBaseData>
                {/*{ingestionTracking.supplierId}*/}
                EMIS
            </TableBaseData>
            <TableBaseData>
                {trimString(ingestionTracking.encryptedFileName)}
                <br />
                <Badge pill bg="success text-white">Re-land</Badge> &nbsp;
                <Badge pill bg="success text-white">Decrypt</Badge>
               
              

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