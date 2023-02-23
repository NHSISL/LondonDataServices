import React, { FunctionComponent } from "react";
import "./SummaryListBaseStyle.css"

interface SummaryListBaseActionProps {
    children?: React.ReactNode;
}

const SummaryListBaseAction: FunctionComponent<SummaryListBaseActionProps> = (props) => {
    return (
        <dd className="summaryListActions">
            {props.children}
        </dd>
    )
}

export default SummaryListBaseAction

