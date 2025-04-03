import React, { FunctionComponent } from "react";
import "./SummaryListBaseStyle.css"

interface SummaryListBaseValueProps {
    children?: React.ReactNode;
}

const SummaryListBaseValue: FunctionComponent<SummaryListBaseValueProps> = (props) => {
    return (
        <dd className="summaryListValue">
            {props.children}
        </dd>
    )
}

export default SummaryListBaseValue

