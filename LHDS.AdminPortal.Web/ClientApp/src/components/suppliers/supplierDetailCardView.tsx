import React from "react";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";

const SupplierDetailCardView = () => {

    return (
        <div>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>File Name</SummaryListBaseKey>
                    <SummaryListBaseValue>/emisnightingale-data-preprod-provider-extracts/IM1/sftp/70CD5674-1F0E-44E8-95C5-75D70EA9A291/20230109/delta_76356_Admin_Location_20230109132842_70CD5674-1F0E-44E8-95C5-75D70EA9A291.csv.gpg
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
            </SummaryListBase>
        </div>
    );
}
export default SupplierDetailCardView;