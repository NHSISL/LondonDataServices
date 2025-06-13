import React, { FunctionComponent } from "react";
import "./SummaryListBaseStyle.css"

interface SummaryListBaseValueProps {
    children?: React.ReactNode;
}

const SummaryListBaseValue: FunctionComponent<SummaryListBaseValueProps> = (props) => {
    return (
        <div className="summaryListValue">
            {props.children}
        </div>
    )
}

export default SummaryListBaseValue

