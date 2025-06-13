import React, { FunctionComponent } from "react";
import "./SummaryListBaseStyle.css"

interface CardBaseProps {
    children?: React.ReactNode;
}

const SummaryListBase: FunctionComponent<CardBaseProps> = (props) => {
    return (
        <div className="dlBase">
            {props.children}
        </div>
    )
}

export default SummaryListBase

