import React, { FunctionComponent } from "react";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import ButtonBase from "../bases/buttons/ButtonBase";
import { Address } from "../../models/addresses/address";

type AddressSearchRowProps = {
    address: Address;
    onPick: (ordinanceAddress: string) => void;
}

const AddressSearchRowView: FunctionComponent<AddressSearchRowProps> = (props) => {
    const {
        address,
        onPick
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
                <span style={{ fontSize: '14px' }}>{address.uprn}</span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: '14px' }}>{address.upsn}</span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: '14px' }}>{concateAddresses(address)}</span>
            </TableBaseData>
            <TableBaseData classes="text-center">
                <ButtonBase onClick={() => onPick(concateAddresses(address))} remove>Pick</ButtonBase>
                {/*<ButtonBase onClick={() => { }} view title="test against assign Api" disabled={true}> Test </ButtonBase>*/}
            </TableBaseData>
        </TableBaseRow>
    );
}

export default AddressSearchRowView;