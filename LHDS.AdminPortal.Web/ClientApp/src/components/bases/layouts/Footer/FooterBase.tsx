import React, { FunctionComponent } from "react";
import "./FooterStyle.css"

interface FooterBaseProps {
    children?: React.ReactNode;
    classes?: string;
}

const FooterBase: FunctionComponent<FooterBaseProps> = (props) => {
    return (
        <footer className={props.classes + " toggled"}>
            {props.children}
        </footer>
    )
}

export default FooterBase

