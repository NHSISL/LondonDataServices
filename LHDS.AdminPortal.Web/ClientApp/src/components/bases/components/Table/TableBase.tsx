import React, { FunctionComponent } from "react";
import "./TableBaseStyle.css"

interface TableBaseProps {
    children?: React.ReactNode;
}

const TableBase: FunctionComponent<TableBaseProps> = (props) => {
    return (
        <table className="table table-bordered">
            {props.children}
        </table>
    )
}

export default TableBase

