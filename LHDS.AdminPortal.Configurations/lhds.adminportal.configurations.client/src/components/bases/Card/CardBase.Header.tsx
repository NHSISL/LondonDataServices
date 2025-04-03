import React, { FunctionComponent } from "react";
import { } from 'nhsuk-react-components'
import "./CardBaseStyle.css"

interface CardBaseHeaderProps {
    children?: React.ReactNode;
}

const CardBaseHeader: FunctionComponent<CardBaseHeaderProps> = (props) => {
    return (
        <div>
            {props.children}
        </div>
    )
}

export default CardBaseHeader

