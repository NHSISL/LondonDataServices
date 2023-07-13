import React, { FunctionComponent } from "react";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";

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
                <br />
                <ButtonBase onClick={() => onRelanding(ingestionTracking)} add>
                    Re-land
                </ButtonBase>

                <ButtonBase onClick={() => onRedecrypt(ingestionTracking)} add>
                    Re-decrypt
                </ButtonBase>

                <ButtonBase onClick={() => onEncryptedDownload(ingestionTracking)} add>
                    Download Encrypted File
                </ButtonBase>

                {ingestionTracking.decrypted &&
                    <ButtonBase onClick={() => onDecryptedDownload(ingestionTracking)} add>
                        Download Decrypted File
                    </ButtonBase>
                }
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