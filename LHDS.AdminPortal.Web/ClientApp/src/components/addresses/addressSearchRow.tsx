import React, { FunctionComponent } from "react";
import { Address } from "../../models/addresses/address";
import AddressSearchRowView from "./addressSearchRowView";

type AddressSearchRowProps = {
    address: Address;
};

const AddressRow: FunctionComponent<AddressSearchRowProps> = (props) => {
    const {
        address
    } = props;

    return (
        <AddressSearchRowView
            key={address.id.toString()}
            address={address}
        />
    );
};

export default AddressRow;