import moment from "moment";
import React, { FunctionComponent } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import ButtonBase from "../../bases/buttons/ButtonBase";
import SummaryListBase from "../../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseKey from "../../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseRow from "../../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../../bases/components/SummaryList/SummaryListBase.Value";

interface OptOutDetailCardViewProps {
    optOuts: Array<OptOutView>;
    onClearCache: (optOuts: Array<OptOutView>) => void;
}

const OptOutDetailCardView: FunctionComponent<OptOutDetailCardViewProps> = (props) => {
    const {
        optOuts,
        onClearCache
    } = props;

    return (
        <>
            {optOuts[0] !== undefined && (
                <div>
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
                                <ButtonBase onClick={() => onClearCache(optOuts)} add>&nbsp;Clear Cache</ButtonBase>&nbsp;
                            </SummaryListBaseValue>
                        </SummaryListBaseRow>
                    </SummaryListBase>
                </div>
            )}

            {optOuts[0] === undefined && (
                <div>
                <p>You can search for an NHS number in the search bar above.   </p>
                <p>Alternatively, if you need to add a new patient to the system, click the "Add new" button to enter some details, including a valid NHS number. 
                 It's important to ensure that all NHS numbers entered into the system are valid and accurate to ensure that patient records are up to date and accessible when needed.
                 </p>

                    <ButtonBase onClick={() => (optOuts)} view>&nbsp;Add New Nhs Number</ButtonBase>
                </div>
            )}
        </>
    );
}

export default OptOutDetailCardView;