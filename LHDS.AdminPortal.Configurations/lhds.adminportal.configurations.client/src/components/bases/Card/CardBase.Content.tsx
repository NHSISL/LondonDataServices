import React, { FunctionComponent } from "react";
import { } from 'nhsuk-react-components'
import "./CardBaseStyle.css"

interface CardBaseContentProps {
    children?: React.ReactNode;
    classes?: string;
}

const CardBaseContent: FunctionComponent<CardBaseContentProps> = (props) => {
    return (
        <div className={props.classes + " cardBaseContent"}>
            {props.children}
        </div>
    )
}

export default CardBaseContent

