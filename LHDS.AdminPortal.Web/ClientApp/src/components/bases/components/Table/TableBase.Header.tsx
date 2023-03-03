import React, { FunctionComponent } from "react";
import "./TableBaseStyle.css"

interface TableBaseHeaderProps {
    children?: React.ReactNode;
    classes?: string;
}

const TableBaseHeader: FunctionComponent<TableBaseHeaderProps> = (props) => {
    return (
        <th className={props.classes }>
            {props.children}
        </th>
    )
}

export default TableBaseHeader

