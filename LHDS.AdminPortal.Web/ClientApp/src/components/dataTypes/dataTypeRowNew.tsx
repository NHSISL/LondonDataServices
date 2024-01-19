import React, { FunctionComponent } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCirclePlus } from '@fortawesome/free-solid-svg-icons'
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { SecuredComponents } from "../links";

interface DataTypeRowNewProps {
    onAdd: (value: boolean) => void;
}

const DataTypeRowNew: FunctionComponent<DataTypeRowNewProps> = (props) => {
    const {
        onAdd
    } = props;

    return (
        <SecuredComponents>
            <TableBaseRow>
                <TableBaseData></TableBaseData>
                <TableBaseData></TableBaseData>
                <TableBaseData classes="text-end">
                    <ButtonBase id="dataTypeAdd" onClick={() => onAdd(true)} add>
                        <FontAwesomeIcon icon={faCirclePlus} size="lg" />&nbsp; New
                    </ButtonBase>
                </TableBaseData>
            </TableBaseRow>
        </SecuredComponents>
    );
}

export default DataTypeRowNew;
