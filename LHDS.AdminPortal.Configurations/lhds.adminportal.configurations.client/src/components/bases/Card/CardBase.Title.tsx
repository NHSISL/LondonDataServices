import React, { FunctionComponent } from "react";
import { } from 'nhsuk-react-components'
import { Card } from "react-bootstrap"
import "./CardBaseStyle.css"

interface CardBaseTitleProps {
    children?: React.ReactNode;
}

const CardBaseTitle: FunctionComponent<CardBaseTitleProps> = (props) => {
    return (
        <Card.Title className="cardBaseTitle">
            {props.children}
       </Card.Title>
    )
}

export default CardBaseTitle

