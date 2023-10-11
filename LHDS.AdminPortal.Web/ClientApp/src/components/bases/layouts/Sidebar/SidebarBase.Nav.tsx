import React, { FunctionComponent } from "react";
import "./SidebarStyle.css"

interface SidebarBaseNavProps {
    children?: React.ReactNode;
    classes?: string;
}

const SidebarBaseNav: FunctionComponent<SidebarBaseNavProps> = (props) => {
    return (
        <ul id="sidebar-nav" className={props.classes + " sidebar-nav"}>
            {props.children}
        </ul>
    )
}

export default SidebarBaseNav

