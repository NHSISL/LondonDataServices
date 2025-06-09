import { Guid } from "guid-typescript";
import React, { FunctionComponent} from "react";
import { addressViewService } from "../../services/views/addresses/addressViewService";
import { AddressView } from "../../models/views/components/addresses/addressView";
import AddressDetailCard from "./addressDetailCard";

interface AddressDetailProps {
    addressId: string;
    children?: React.ReactNode;
}

const AddressDetail: FunctionComponent<AddressDetailProps> = (props) => {
    const {
        addressId,
        children
    } = props;

     const { mappedAddress: addressRetrieved } =
        addressViewService.useGetAddressById(Guid.parse(addressId))

    const handleRefresh = async (addressView: AddressView) => { }

    return (
        <div>
            {addressRetrieved !== undefined && (
                <div>
                    <AddressDetailCard
                        key={addressRetrieved.id.toString()}
                        address={addressRetrieved}
                        onRefresh={handleRefresh}>                   
                        {children}
                    </AddressDetailCard>
                </div>
            )}
        </div>
    );
}

export default AddressDetail;