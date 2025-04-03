import React, { FunctionComponent } from "react";
import "./TableBaseStyle.css"

interface TableBaseRowProps {
    children?: React.ReactNode;
    classes?: string
}

const TableBaseRow: FunctionComponent<TableBaseRowProps> = (props) => {
    return (
        <tr className={props.classes}>
            {props.children}
        </tr>
    )
}

export default TableBaseRow

