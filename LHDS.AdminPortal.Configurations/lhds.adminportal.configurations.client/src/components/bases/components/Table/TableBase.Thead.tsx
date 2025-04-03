import React, { FunctionComponent } from "react";
import "./TableBaseStyle.css"

interface TableBaseTheadProps {
    children?: React.ReactNode;
}

const TableBaseThead: FunctionComponent<TableBaseTheadProps> = (props) => {
    return (
        <thead>
            {props.children}
        </thead>
    )
}

export default TableBaseThead

