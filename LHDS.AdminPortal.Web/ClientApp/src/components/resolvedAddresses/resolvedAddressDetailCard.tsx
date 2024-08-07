import React, { FunctionComponent } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import { ResolvedAddressView } from "../../models/views/components/resolvedAddresses/resolvedAddressView";
import ResolvedAddressDetailCardView from "./resolvedAddressDetailCardView";

interface ResolvedAddressDetailCardProps {
    resolvedAddress: ResolvedAddressView;
    children?: React.ReactNode;
    onRefresh: (resolvedAddress: ResolvedAddressView) => void;
    onUpdate: (resolvedAddress: ResolvedAddressView,) => void;
}

const ResolvedAddressDetailCard: FunctionComponent<ResolvedAddressDetailCardProps> = (props) => {
    const {
        resolvedAddress,
        children,
        onRefresh,
        onUpdate
    } = props;

    const handlRefresh = async (resolvedAddress: ResolvedAddressView) => {
        await onRefresh(resolvedAddress);
    }

    const handleUpdate = async (resolvedAddress: ResolvedAddressView) => {
        await onUpdate(resolvedAddress);
    };

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Address Details
                    </CardBaseTitle>
                    <CardBaseContent>
                        <ResolvedAddressDetailCardView
                            resolvedAddress={resolvedAddress}
                            onRefresh={handlRefresh}
                            onUpdate={handleUpdate}
                        />

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

export default ResolvedAddressDetailCard;