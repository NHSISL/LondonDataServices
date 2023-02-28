import React, { FunctionComponent } from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";

interface IngestionTrackingDetailCardViewProps {
    ingestionTracking: IngestionTrackingView;
}

const IngestionTrackingDetailCardView: FunctionComponent<IngestionTrackingDetailCardViewProps> = (props) => {
    const {
        ingestionTracking
    } = props;

    return (
        <>
            <h1>Details</h1>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Encrypted FileName</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.encryptedFileName}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Decrypted FileName</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.decryptedFileName}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Decrypted</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.decrypted}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Seen</SummaryListBaseKey>
                    <SummaryListBaseValue></SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>File Deleted</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.fileDeleted}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Record Count</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.recordCount}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Encrypted FileSize</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.encryptedFileSize}</SummaryListBaseValue>
                </SummaryListBaseRow>
            </SummaryListBase>
        </>
    );
}
export default IngestionTrackingDetailCardView;