import React, { FunctionComponent } from "react";
import "./SummaryListBaseStyle.css"

interface CardBaseProps {
    children?: React.ReactNode;
}

const SummaryListBase: FunctionComponent<CardBaseProps> = (props) => {
    return (
        <dl className="dlBase">
            {props.children}
        </dl>
    )
}

export default SummaryListBase

