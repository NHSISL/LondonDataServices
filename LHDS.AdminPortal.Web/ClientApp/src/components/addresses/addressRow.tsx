import React, { FunctionComponent } from "react";
import AddressRowView from "./addressRowView";
import { Address } from "../../models/addresses/address";

type AddressRowProps = {
    address: Address;
};

const AddressRow: FunctionComponent<AddressRowProps> = (props) => {
    const {
        address
    } = props;

    return (
        <AddressRowView
            key={address.id.toString()}
            address={address}
        />
    );
};

export default AddressRow;