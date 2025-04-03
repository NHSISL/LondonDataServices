import React, { FunctionComponent } from "react";
import "./TableBaseStyle.css"

interface TableBaseTbodyProps {
    children?: React.ReactNode;
}

const TableBaseTbody: FunctionComponent<TableBaseTbodyProps> = (props) => {
    return (
        <thead>
            {props.children}
        </thead>
    )
}

export default TableBaseTbody

