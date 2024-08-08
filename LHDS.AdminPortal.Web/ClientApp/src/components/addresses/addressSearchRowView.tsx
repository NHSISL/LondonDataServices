import React, { FunctionComponent } from "react";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import ButtonBase from "../bases/buttons/ButtonBase";
import { Address } from "../../models/addresses/address";

type AddressSearchRowProps = {
    address: Address;
}

const AddressSearchRowView: FunctionComponent<AddressSearchRowProps> = (props) => {
    const {
        address
    } = props;


    const concateAddresses = (address: Address) => {
        const parts = [
            address.organisationName,
            address.departmentName,
            address.subBuildingName,
            address.buildingName,
            address.buildingNumber,
            address.dependentThoroughfare,
            address.thoroughfare,
            address.doubleDependentLocality,
            address.dependentLocality,
            address.postTown,
            address.postCode
        ];

        return parts.filter(part => part).join(', ');
    }

    return (
        <TableBaseRow>
            <TableBaseData>
                <span>{address.uprn}</span>
            </TableBaseData>
            <TableBaseData>
                <span>{address.upsn}</span>
            </TableBaseData>
            <TableBaseData>
                <span>{concateAddresses(address)}</span>
            </TableBaseData>
            <TableBaseData classes="text-center">
                <ButtonBase onClick={() => { }} add> Pick </ButtonBase>
                <ButtonBase onClick={() => { }} view title="test against assign Api" disabled={true}> Test </ButtonBase>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default AddressSearchRowView;