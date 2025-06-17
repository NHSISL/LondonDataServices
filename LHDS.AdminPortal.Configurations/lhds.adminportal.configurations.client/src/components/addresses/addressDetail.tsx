import React, { FunctionComponent} from "react";
import { addressViewService } from "../../services/views/addresses/addressViewService";
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
        addressViewService.useGetAddressById(addressId);

    return (
        <div>
            {addressRetrieved !== undefined && (
                <div>
                    <AddressDetailCard
                        key={addressRetrieved.id.toString()}
                        address={addressRetrieved}>                   
                        {children}
                    </AddressDetailCard>
                </div>
            )}
        </div>
    );
}

export default AddressDetail;