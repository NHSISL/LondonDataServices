import React, { FunctionComponent } from "react";
import { ResolvedAddress } from "../../models/resolvedAddresses/resolvedAddress";
import ResolvedAddressRowView from "./resolvedAddressRowView";

type ResolvedAddressRowProps = {
    resolvedAddress: ResolvedAddress;
};

const ResolvedAddressRow: FunctionComponent<ResolvedAddressRowProps> = (props) => {
    const {
        resolvedAddress
    } = props;

    return (
        <ResolvedAddressRowView
            key={resolvedAddress.id.toString()}
            resolvedAddress={resolvedAddress}
        />
    );
};

export default ResolvedAddressRow;