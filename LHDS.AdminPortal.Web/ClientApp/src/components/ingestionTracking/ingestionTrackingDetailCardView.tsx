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
                    <SummaryListBaseKey>File Name</SummaryListBaseKey>
                    <SummaryListBaseValue>{ingestionTracking.encryptedFileName}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Decrypted</SummaryListBaseKey>
                    <SummaryListBaseValue>True</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Encrypted File Size</SummaryListBaseKey>
                    <SummaryListBaseValue>1000mb</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Decrypted File Size</SummaryListBaseKey>
                    <SummaryListBaseValue>500mb</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Record Count</SummaryListBaseKey>
                    <SummaryListBaseValue>22</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Seen</SummaryListBaseKey>
                    <SummaryListBaseValue>22-Aug-2022</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Created Date</SummaryListBaseKey>
                    <SummaryListBaseValue>22-Aug-2022</SummaryListBaseValue>
                </SummaryListBaseRow>
            </SummaryListBase>
        </>
    );
}
export default IngestionTrackingDetailCardView;