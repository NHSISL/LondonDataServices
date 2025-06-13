import React, { FunctionComponent } from "react";
import "./SummaryListBaseStyle.css"

interface SummaryListBaseKeyProps {
    children?: React.ReactNode;
    classes?: string;
}

const SummaryListBaseKey: FunctionComponent<SummaryListBaseKeyProps> = (props) => {
    return (
        <div className="summaryListKey">
            <span className={props.classes }>
                {props.children}
            </span>
        </div>
    )
}

export default SummaryListBaseKey

