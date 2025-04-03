import React, { FunctionComponent } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCirclePlus } from '@fortawesome/free-solid-svg-icons'
import ButtonBase from "../bases/buttons/ButtonBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { SecuredComponents } from "../links";

interface SupplierRowNewProps {
    onAdd: (value: boolean) => void;
}

const SupplierRowNew: FunctionComponent<SupplierRowNewProps> = (props) => {
    const {
        onAdd
    } = props;

    return (
        <SecuredComponents>
            <TableBaseRow>
                <TableBaseData></TableBaseData>
                <TableBaseData></TableBaseData>
                <TableBaseData classes="text-end">
                    <ButtonBase id="supplierssAdd" onClick={() => onAdd(true)} add>
                        <FontAwesomeIcon icon={faCirclePlus} size="lg" />&nbsp; New
                    </ButtonBase>
                </TableBaseData>
            </TableBaseRow>
        </SecuredComponents>
    );
}

export default SupplierRowNew;
