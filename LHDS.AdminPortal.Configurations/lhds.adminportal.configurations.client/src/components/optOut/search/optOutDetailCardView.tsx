import moment from "moment";
import React, { FunctionComponent } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import ButtonBase from "../../bases/buttons/ButtonBase";
import SummaryListBase from "../../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseKey from "../../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseRow from "../../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../../bases/components/SummaryList/SummaryListBase.Value";
import securityPoints from "../../../securityMatrix";
import { SecuredComponents } from "../../links";

interface OptOutDetailCardViewProps {
    optOuts: OptOutView | undefined;
    onClearCache: (optOuts: OptOutView) => void;
    onAddNewNHS: (optOuts: OptOutView) => void;
    nhsNumber: string,
    isValidNumber: boolean
}

const OptOutDetailCardView: FunctionComponent<OptOutDetailCardViewProps> = (props) => {
    const {
        optOuts,
        onClearCache,
        onAddNewNHS,
        nhsNumber,
        isValidNumber
    } = props;

    if (!optOuts) {
        return <div>
            {!isValidNumber &&
                <div>
                    <strong>Note: </strong>
                    Please ensure you have typed a valid nhs number
                </div>
            }</div>
    }

    if (optOuts.id.toString() !== "") {
        return <div>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>NHS Number</SummaryListBaseKey>
                    <SummaryListBaseValue><strong>{optOuts.nhsNumber}</strong></SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Opt-Out Status</SummaryListBaseKey>
                    <SummaryListBaseValue>{optOuts.status}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Batch Reference</SummaryListBaseKey>
                    <SummaryListBaseValue>{optOuts.batchReference}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Unique Reference</SummaryListBaseKey>
                    <SummaryListBaseValue>{optOuts.uniqueReference}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Cache Time</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {optOuts.cacheTime ? moment(optOuts.cacheTime?.toString()).format("Do-MMM-yyyy HH:mm") : ""}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Last Sent To Mesh</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {optOuts.lastSentToMesh ? moment(optOuts.lastSentToMesh?.toString()).format("Do-MMM-yyyy HH:mm") : ""}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Created By</SummaryListBaseKey>
                    <SummaryListBaseValue>{optOuts.createdBy}</SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Created When</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        {optOuts.createdDate ? moment(optOuts.createdDate?.toString()).format("Do-MMM-yyyy HH:mm") : ""}
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                <SummaryListBaseRow>
                    <SummaryListBaseKey></SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <ButtonBase onClick={() => onClearCache(optOuts)} add>&nbsp;Clear Cache</ButtonBase>&nbsp;
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
            </SummaryListBase>
        </div>
    }

    if (optOuts !== undefined && optOuts.id.toString() === "") {
        return <div>
            <SecuredComponents allowedRoles={securityPoints.optOut.add}>
                <ButtonBase onClick={onAddNewNHS} view>Add {nhsNumber} to NDOP check</ButtonBase>
            </SecuredComponents>
            <SecuredComponents allowedRoles={securityPoints.optOut.readonly}>
                <p>You do not have permission to add NHS Number: <strong>{nhsNumber}</strong></p>
            </SecuredComponents>
        </div>
    }

    return <></>;
}

export default OptOutDetailCardView;