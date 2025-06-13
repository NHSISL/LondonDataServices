import React, { FunctionComponent } from "react";
import "./SummaryListBaseStyle.css"

interface SummaryListBaseActionProps {
    children?: React.ReactNode;
}

const SummaryListBaseAction: FunctionComponent<SummaryListBaseActionProps> = (props) => {
    return (
        <div className="summaryListActions">
            {props.children}
        </div>
    )
}

export default SummaryListBaseAction

