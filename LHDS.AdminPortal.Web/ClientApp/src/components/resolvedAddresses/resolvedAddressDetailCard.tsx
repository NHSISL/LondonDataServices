import React, { FunctionComponent, useState } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import { ResolvedAddressView } from "../../models/views/components/resolvedAddresses/resolvedAddressView";
import ResolvedAddressDetailCardView from "./resolvedAddressDetailCardView";
import ResolvedAddressDetailCardEdit from "./resolvedAddressDetailCardEdit";
import AddressSearchTable from "../addresses/addressSearchTable";

interface ResolvedAddressDetailCardProps {
    resolvedAddress: ResolvedAddressView;
    mode: string;
    children?: React.ReactNode;
    onRefresh: (resolvedAddress: ResolvedAddressView) => void;
    onUpdate: (resolvedAddress: ResolvedAddressView) => void;
}

const ResolvedAddressDetailCard: FunctionComponent<ResolvedAddressDetailCardProps> = (props) => {
    const {
        resolvedAddress,
        mode,
        children,
        onRefresh,
        onUpdate
    } = props;

    const [displayMode, setDisplayMode] = useState<string>(mode);
    const [currentResolvedAddress, setCurrentResolvedAddress] = useState<ResolvedAddressView>({ ...resolvedAddress });

    const handleModeChange = (value: string) => {
        setDisplayMode(value);
    };

    const handlRefresh = async (resolvedAddress: ResolvedAddressView) => {
        await onRefresh(resolvedAddress);
    }

    const handleUpdate = async (resolvedAddress: ResolvedAddressView) => {
        await onUpdate(resolvedAddress);
    };

    const handleCancel = () => {
        
    }

    const handlePick = (address: string) => {
        const updatedResolvedAddress = { ...currentResolvedAddress, alternateUnstructuredPostalAddress: address };
        setCurrentResolvedAddress(updatedResolvedAddress);
        onRefresh(updatedResolvedAddress);
    };

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Resolved Address Details
                    </CardBaseTitle>
                    <CardBaseContent>

                        {(displayMode === "VIEW" || displayMode === "CONFIRMDELETE") && (
                            <ResolvedAddressDetailCardView
                                resolvedAddress={currentResolvedAddress}
                                onRefresh={handlRefresh}
                                onModeChange={handleModeChange}
                            />
                        )}
                        {(displayMode === "EDIT" || displayMode === "ADD") && (
                            <>
                                <ResolvedAddressDetailCardEdit
                                    onModeChange={handleModeChange}
                                    onUpdate={handleUpdate}
                                    onCancel={handleCancel}
                                    resolvedAddress={currentResolvedAddress}
                                    mode={displayMode}/>

                                <AddressSearchTable
                                    onPick={handlePick}
                                ></AddressSearchTable>
                            </>
                        )}

                        {children !== undefined && (
                            <>
                                <br />
                                {children}
                            </>
                        )}
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div >
    );
}

export default ResolvedAddressDetailCard;