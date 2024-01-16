import React, { FunctionComponent } from "react";
import "./HeaderStyle.css"

interface HeaderBaseNavProps {
    children?: React.ReactNode;
    classes?: string;
}

const HeaderBaseNav: FunctionComponent<HeaderBaseNavProps> = (props) => {
    return (
        <nav id="headerbasenav" className={props.classes + " navbar navbar-expand-lg navbar-light bg-light"}>
            <div className={props.classes + " container-fluid"}>
                {props.children}
            </div>
        </nav>
    )
}

export default HeaderBaseNav
