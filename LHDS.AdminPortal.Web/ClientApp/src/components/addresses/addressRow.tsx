import React, { FunctionComponent } from "react";
import { Address } from "../../models/addresses/address";
import AddressRowView from "./addressRowView";

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