import React from "react";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";

const IngestionTrackingDetailCardView = () => {

    return (
        <div>
            <h1>Details</h1>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>File Name</SummaryListBaseKey>
                    <SummaryListBaseValue>/emisnightingale-data-preprod-provider-extracts/IM1/sftp/70CD5674-1F0E-44E8-95C5-75D70EA9A291/20230109/delta_76356_Admin_Location_20230109132842_70CD5674-1F0E-44E8-95C5-75D70EA9A291.csv.gpg </SummaryListBaseValue>
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
        </div>
    );
}
export default IngestionTrackingDetailCardView;