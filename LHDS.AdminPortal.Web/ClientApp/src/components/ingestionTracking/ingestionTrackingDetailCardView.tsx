import { faCheck, faTimes, faFileDownload } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { FunctionComponent } from "react";
import moment from "moment";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import ButtonBase from "../bases/buttons/ButtonBase";

interface IngestionTrackingDetailCardViewProps {
    ingestionTracking: IngestionTrackingView;
    onDownload: (ingestionTracking: IngestionTrackingView) => void;
    onReLand: (ingestionTracking: IngestionTrackingView) => void;
    onReDecrypt: (ingestionTracking: IngestionTrackingView) => void;
}

const IngestionTrackingDetailCardView: FunctionComponent<IngestionTrackingDetailCardViewProps> = (props) => {
    const {
        ingestionTracking,
        onDownload,
        onReLand,
        onReDecrypt
    } = props;

    return (
        <>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.supplier?.name}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Encrypted File Name</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.encryptedFileName}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Decrypted File Name</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.decryptedFileName}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Decrypted</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.decrypted ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Seen</SummaryListBaseKey>
                    <SummaryListBaseValue>{moment(ingestionTracking.lastSeen?.toString()).format("Do-MMM-yyyy HH:mm")}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>File Deleted</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.fileDeleted ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Record Count</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.recordCount}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Encrypted File Size</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.encryptedFileSize}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Decrypted File Size</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.decryptedFileSize}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Created Date</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {moment(ingestionTracking.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Updated Date</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {moment(ingestionTracking.updatedDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Actions</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <ButtonBase onClick={() => onReLand(ingestionTracking)} add>&nbsp;Re-Land</ButtonBase>&nbsp;
                        <ButtonBase onClick={() => onReDecrypt(ingestionTracking)} add>&nbsp;Re-Decrypt</ButtonBase>&nbsp;
                        <ButtonBase onClick={() => onDownload(ingestionTracking)} add>
                            <FontAwesomeIcon icon={faFileDownload} />&nbsp;Download
                        </ButtonBase>&nbsp;
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
            </SummaryListBase>


        </>
    );
}
export default IngestionTrackingDetailCardView;