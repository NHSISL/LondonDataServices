import React, { FunctionComponent } from "react";
import { Card } from "react-bootstrap"
import "./CardBaseStyle.css"

interface CardBaseProps {
    children?: React.ReactNode;
    classes?: string;
}

const CardBase: FunctionComponent<CardBaseProps> = (props) => {
    return (
        <Card className={props.classes + " cardBase mh-200"}>
            {props.children}
        </Card>
    )
}

export default CardBase

