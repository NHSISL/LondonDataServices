import moment from "moment";
import React, { FunctionComponent} from "react";
import { OptOutView } from "../../models/views/components/optOuts/optOutView";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";

interface OptOutDetailCardViewProps {
    optOuts: Array<OptOutView>;
}

const OptOutDetailCardView: FunctionComponent<OptOutDetailCardViewProps> = (props) => {
    const {
        optOuts,
    } = props;
    return (
        <>
            <p>Please Search for an NHS Number above to view the details.</p>
            {optOuts[0] !== undefined && (
                <SummaryListBase>
                    <SummaryListBaseRow>
                        <SummaryListBaseKey>NHS Number</SummaryListBaseKey>
                        <SummaryListBaseValue>{optOuts[0].nhsNumber}</SummaryListBaseValue>
                    </SummaryListBaseRow>
                    <SummaryListBaseRow>
                        <SummaryListBaseKey>Opt-Out Status</SummaryListBaseKey>
                        <SummaryListBaseValue>{optOuts[0].optOutStatus}</SummaryListBaseValue>
                    </SummaryListBaseRow>
                    <SummaryListBaseRow>
                        <SummaryListBaseKey>Cache Time</SummaryListBaseKey>
                        <SummaryListBaseValue>{moment(optOuts[0].cacheTime?.toString()).format("Do-MMM-yyyy")}</SummaryListBaseValue>
                    </SummaryListBaseRow>
                    <SummaryListBaseRow>
                        <SummaryListBaseKey>Last Sent To Mesh</SummaryListBaseKey>
                        <SummaryListBaseValue>{moment(optOuts[0].lastSentToMesh?.toString()).format("Do-MMM-yyyy")}</SummaryListBaseValue>
                    </SummaryListBaseRow>

                    <SummaryListBaseRow>
                        <SummaryListBaseKey></SummaryListBaseKey>
                        <SummaryListBaseValue>
                            <ButtonBase onClick={() => (optOuts)} add>&nbsp;Clear Cache</ButtonBase>&nbsp;

                        </SummaryListBaseValue>
                    </SummaryListBaseRow>
                </SummaryListBase>
            )}
        </>
    );
}
export default OptOutDetailCardView;