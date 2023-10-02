import React, { FunctionComponent } from "react";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFileImport, faLockOpen, faFileDownload, faFileExport, faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';
import { Dropdown } from "react-bootstrap";
import moment from "moment";

type IngestionTrackingRowProps = {
    onRelanding: (ingestionTracking: IngestionTracking) => void;
    onRedecrypt: (ingestionTracking: IngestionTracking) => void;
    onEncryptedDownload: (ingestionTracking: IngestionTracking) => void;
    onDecryptedDownload: (ingestionTracking: IngestionTracking) => void;
    ingestionTracking: IngestionTracking;
}

const IngestionTrackingRow: FunctionComponent<IngestionTrackingRowProps> = (props) => {
    const {
        onRelanding,
        onRedecrypt,
        onEncryptedDownload,
        onDecryptedDownload,
        ingestionTracking
    } = props;

    function trimString(fileName: string) {
        const str = fileName;
        const lastIndex = str.lastIndexOf('/');

        if (lastIndex !== -1) {
            const secondLastIndex = str.lastIndexOf('/', lastIndex - 1);
            if (secondLastIndex !== -1) {
                return str.substring(secondLastIndex + 1, str.length - 4);
            }
        }

        return fileName;
    }

    return (
        <TableBaseRow>
            <TableBaseData>
                <div
                    className="text-white p-2 rounded al text-center"
                    style={{ backgroundColor: ingestionTracking.supplier?.name === 'EMIS' ? '#0984e3' : '#00b894' }}>
                    {ingestionTracking.supplier?.name}
                </div>
            </TableBaseData>
            <TableBaseData>
                FileName: {trimString(ingestionTracking.fileName)}
                <br />
                Decrypted: {ingestionTracking.decrypted ? <FontAwesomeIcon icon={faCheck} className="text-success" />
                    : <FontAwesomeIcon icon={faTimes} className="text-danger" />} &nbsp;

                Deleted: {ingestionTracking.fileDeleted ? <FontAwesomeIcon icon={faCheck} className="text-success" />

                    : <FontAwesomeIcon icon={faTimes} className="text-danger" />} &nbsp;
                Record Count: {ingestionTracking.recordCount} &nbsp;
                
            </TableBaseData>
            <TableBaseData>
                <strong>Created Date: </strong><br/>
                {moment(ingestionTracking.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
            </TableBaseData>
            <TableBaseData>
                <Dropdown>
                    <Dropdown.Toggle as={ButtonBase} variant="secondary" id="actions-dropdown" edit>
                        Actions
                    </Dropdown.Toggle>

                    <Dropdown.Menu>
                        <Dropdown.Item onClick={() => onRelanding(ingestionTracking)}
                            style={{ color: "#121212" }}>
                            <FontAwesomeIcon icon={faFileImport} title="Re-Land" /> Re-Land File
                        </Dropdown.Item>
                        <Dropdown.Item onClick={() => onRedecrypt(ingestionTracking)}
                            style={{ color: "#121212" }}>
                            <FontAwesomeIcon icon={faLockOpen} /> Re-Decrypt File
                        </Dropdown.Item>
                        <Dropdown.Item onClick={() => onEncryptedDownload(ingestionTracking)}
                            style={{ color: "#121212" }}>
                            <FontAwesomeIcon icon={faFileDownload} />  &nbsp;Download Encrypted File
                        </Dropdown.Item>
                        {ingestionTracking.decrypted && (
                            <Dropdown.Item onClick={() => onDecryptedDownload(ingestionTracking)}
                                style={{ color: "#121212" }}>
                                <FontAwesomeIcon icon={faFileExport} />  Download Decrypted File
                            </Dropdown.Item>
                        )}
                    </Dropdown.Menu>
                </Dropdown>
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