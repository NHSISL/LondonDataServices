import moment from "moment";
import React, { FunctionComponent } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import ButtonBase from "../../bases/buttons/ButtonBase";
import SummaryListBase from "../../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseKey from "../../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseRow from "../../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../../bases/components/SummaryList/SummaryListBase.Value";

interface OptOutDetailCardViewProps {
    optOuts: OptOutView | undefined;
    onClearCache: (optOuts: OptOutView) => void;
    onAddNewNHS: () => void;
}

const OptOutDetailCardView: FunctionComponent<OptOutDetailCardViewProps> = (props) => {
    const {
        optOuts,
        onClearCache,
        onAddNewNHS 
    } = props;

    return (
        <>
            {optOuts !== undefined && (
                <div>
                    <SummaryListBase>
                        <SummaryListBaseRow>
                            <SummaryListBaseKey>NHS Number</SummaryListBaseKey>
                            <SummaryListBaseValue>{optOuts.nhsNumber}</SummaryListBaseValue>
                        </SummaryListBaseRow>
                        <SummaryListBaseRow>
                            <SummaryListBaseKey>Opt-Out Status</SummaryListBaseKey>
                            <SummaryListBaseValue>{optOuts.status}</SummaryListBaseValue>
                        </SummaryListBaseRow>
                        <SummaryListBaseRow>
                            <SummaryListBaseKey>Cache Time</SummaryListBaseKey>
                            <SummaryListBaseValue>
                                {moment(optOuts.cacheTime?.toString()).format("Do-MMM-yyyy")}
                            </SummaryListBaseValue>
                        </SummaryListBaseRow>
                        <SummaryListBaseRow>
                            <SummaryListBaseKey>Last Sent To Mesh</SummaryListBaseKey>
                            <SummaryListBaseValue>
                                {moment(optOuts.lastSentToMesh?.toString()).format("Do-MMM-yyyy")}
                            </SummaryListBaseValue>
                        </SummaryListBaseRow>
                        <SummaryListBaseRow>
                            <SummaryListBaseKey></SummaryListBaseKey>
                            <SummaryListBaseValue>
                                <ButtonBase onClick={() => onClearCache(optOuts)} add>
                                    &nbsp;Clear Cache
                                </ButtonBase>&nbsp;
                            </SummaryListBaseValue>
                        </SummaryListBaseRow>
                    </SummaryListBase>
                </div>
            )}

            {optOuts === undefined && (
                <div>
                <p>You can search for an NHS number in the search bar above.   </p>
                   {/* <ButtonBase onClick={onAddNewNHS} view>&nbsp;Add New Nhs Number</ButtonBase>*/}
                </div>
            )}
        </>
    );
}

export default OptOutDetailCardView;