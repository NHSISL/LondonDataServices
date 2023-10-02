import React, { FunctionComponent } from "react";
import "./TableBaseStyle.css"

interface TableBaseTbodyProps {
    children?: React.ReactNode;
}

const TableBaseTbody: FunctionComponent<TableBaseTbodyProps> = (props) => {
    return (
        <tbody>
            {props.children}
        </tbody>
    )
}

export default TableBaseTbody

