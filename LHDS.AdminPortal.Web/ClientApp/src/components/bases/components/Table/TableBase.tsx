import React, { FunctionComponent } from "react";
import "./TableBaseStyle.css"

interface TableBaseProps {
    children?: React.ReactNode;
    classes?: string
}

const TableBase: FunctionComponent<TableBaseProps> = (props) => {
    return (
        <table className={props.classes + " table table-responsive "}>
            {props.children}
        </table>
    )
}

export default TableBase

