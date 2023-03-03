import React, { FunctionComponent } from "react";
import "./SummaryListBaseStyle.css"

interface SummaryListBaseRowProps {
    children?: React.ReactNode;
    classes?: string;
}

const SummaryListBaseRow: FunctionComponent<SummaryListBaseRowProps> = (props) => {
    return (
        <div className={props.classes + " summaryRow"}>
            {props.children}
        </div>
    )
}

export default SummaryListBaseRow

