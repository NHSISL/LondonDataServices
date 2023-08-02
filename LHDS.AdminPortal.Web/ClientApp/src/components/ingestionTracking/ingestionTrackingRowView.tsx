import React, { FunctionComponent } from "react";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFileImport, faLockOpen, faFileDownload, faFileExport } from '@fortawesome/free-solid-svg-icons';
import { Dropdown, DropdownButton } from "react-bootstrap";

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
        return str.substring(lastIndex + 1, str.length - 4);
    }

    return (
        <TableBaseRow>
            <TableBaseData>
                {ingestionTracking.supplier?.name}
            </TableBaseData>
            <TableBaseData>
                {trimString(ingestionTracking.fileName)}
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