import React, { FunctionComponent } from "react";
import { } from 'nhsuk-react-components'
import { Card } from "react-bootstrap"
import "./CardBaseStyle.css"

interface CardBaseBodyProps {
    children?: React.ReactNode;
    classes?: string;
}

const CardBaseBody: FunctionComponent<CardBaseBodyProps> = (props) => {
    return (
        <Card.Body className={props.classes + " cardBaseBody"}>
            {props.children}
        </Card.Body>
    )
}

export default CardBaseBody

