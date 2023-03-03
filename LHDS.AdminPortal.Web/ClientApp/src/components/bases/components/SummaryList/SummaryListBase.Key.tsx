import React, { FunctionComponent } from "react";
import "./SummaryListBaseStyle.css"

interface SummaryListBaseKeyProps {
    children?: React.ReactNode;
    classes?: string;
}

const SummaryListBaseKey: FunctionComponent<SummaryListBaseKeyProps> = (props) => {
    return (
        <dt className="summaryListKey">
            <span className={props.classes }>
                {props.children}
            </span>
        </dt>
    )
}

export default SummaryListBaseKey

