import React, { FunctionComponent } from "react";
import "./SidebarStyle.css"

interface SidebarBaseProps {
    children?: React.ReactNode;
    classes?: string;
}

const SidebarBase: FunctionComponent<SidebarBaseProps> = (props) => {
    return (
        <div id="sidebar-wrapper" className={props.classes}>
            {props.children}
        </div>
    )
}

export default SidebarBase
