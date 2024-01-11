import React, { FunctionComponent } from "react";
import "./HeaderStyle.css"

interface HeaderBaseProps {
    children?: React.ReactNode;
    classes?: string;
}

const HeaderBase: FunctionComponent<HeaderBaseProps> = (props) => {
    return (
        <header id="toggled" className={props.classes + " toggled"}>
            {props.children}
        </header>
    )
}

export default HeaderBase
