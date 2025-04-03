import React, { FunctionComponent } from "react";
import "./TableBaseStyle.css"

interface TableBaseDataProps {
    children?: React.ReactNode;
    cols?: number;
    classes?: string;
}

const TableBaseData: FunctionComponent<TableBaseDataProps> = (props) => {
    return (
        <td colSpan={props.cols} className={props.classes}>
            {props.children}
        </td>
    )
}

export default TableBaseData

