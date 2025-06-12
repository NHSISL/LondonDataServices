import React, { FunctionComponent } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import { AddressView } from "../../models/views/components/addresses/addressView";
import AddressDetailCardView from "./addressDetailCardView";

interface AddressDetailCardProps {
    address: AddressView;
    children?: React.ReactNode;
    onRefresh: (address: AddressView) => void;
}

const AddressDetailCard: FunctionComponent<AddressDetailCardProps> = (props) => {
    const {
        address,
        children,
        onRefresh
    } = props;

    const handlRefresh = async (address: AddressView) => {
        await onRefresh(address);
    }

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Address Details
                    </CardBaseTitle>
                    <CardBaseContent>
                        <AddressDetailCardView
                            address={address}
                            onRefresh={handlRefresh} />
                        {children !== undefined && (
                            <>
                                <br />
                                {children}
                            </>
                        )}
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
}

export default AddressDetailCard;