import React, { FunctionComponent } from "react";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';
import moment from "moment";
import { Button } from "react-bootstrap";

type IngestionTrackingRowProps = {
    onReDecrypted: (ingestionTracking: IngestionTracking) => void;
    ingestionTracking: IngestionTracking;
    onBatchClick: (batch: string) => void
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
                <div className="p-2 rounded al text-center">
                    {ingestionTracking.supplier?.name}

                </div>
            </TableBaseData>

            <TableBaseData>
                FileName: {trimString(ingestionTracking.fileName)}
                <br />
                <span>
                    Decrypted: {ingestionTracking.decrypted ? <FontAwesomeIcon icon={faCheck} className="text-success" />
                        : <FontAwesomeIcon icon={faTimes} className="text-danger" />} &nbsp;
                </span>
                <span>
                    Record Count: {ingestionTracking.recordCount} &nbsp;
                </span>
            </TableBaseData>

            <TableBaseData>
                <strong>Batch:</strong><br />
                <Button
                    variant="link"
                    onClick={() => props.onBatchClick(ingestionTracking.batch)}
                    className="p-0"
                >
                    {ingestionTracking.batch}
                </Button>
            </TableBaseData>

            <TableBaseData>
                <strong>Created Date: </strong><br />
                {moment(ingestionTracking.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
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