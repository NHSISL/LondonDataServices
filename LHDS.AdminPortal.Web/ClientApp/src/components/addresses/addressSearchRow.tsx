import React, { FunctionComponent } from "react";
import { Address } from "../../models/addresses/address";
import AddressSearchRowView from "./addressSearchRowView";

type AddressSearchRowProps = {
    address: Address;
    onPick: (ordinanceAddress: string) => void;
};

const AddressRow: FunctionComponent<AddressSearchRowProps> = (props) => {
    const {
        address,
        onPick
    } = props;

    const handlePick = (address: string) => {
        onPick(address);
    };

    return (
        <AddressSearchRowView
            key={address.id.toString()}
            address={address}
            onPick={handlePick}
        />
    );
};

export default AddressRow;