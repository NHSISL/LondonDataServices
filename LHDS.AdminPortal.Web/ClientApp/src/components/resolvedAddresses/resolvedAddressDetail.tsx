import { Guid } from "guid-typescript";
import React, { FunctionComponent, useEffect, useState} from "react";
import { resolvedAddressViewService } from "../../services/views/resolvedAddresses/resolvedAddressViewService";
import { ResolvedAddressView } from "../../models/views/components/resolvedAddresses/resolvedAddressView";
import ResolvedAddressDetailCard from "./resolvedAddressDetailCard";

interface ResolvedAddressDetailProps {
    resolvedAddressId: string;
    onPickAlternateAddress: (alternateAddress: string) => void; // Corrected prop
    children?: React.ReactNode;
}

const ResolvedAddressDetail: FunctionComponent<ResolvedAddressDetailProps> = (props) => {
    const {
        resolvedAddressId,
        onPickAlternateAddress,
        children
    } = props;

    //const [resolvedAddress, setResolvedAddress] = useState<ResolvedAddressView>();
    const [mode, setMode] = useState<string>('VIEW');

     const { mappedResolvedAddress: resolvedAddressRetrieved } =
         resolvedAddressViewService.useGetResolvedAddressById(Guid.parse(resolvedAddressId))

    const handleRefresh = async (resolvedAddressView: ResolvedAddressView) => {
        const alternateAddress = resolvedAddressView.alternateUnstructuredPostalAddress ?? "";
        onPickAlternateAddress(alternateAddress);
    }

    const updateResolvedAddress = resolvedAddressViewService.useUpdateResolvedAddress();

    const handleUpdate = async (resolvedAddress: ResolvedAddressView) => {
        resolvedAddress.matchedWithAssign = false;
        resolvedAddress.isExported = false;
        resolvedAddress.isProcessed = false;
        resolvedAddress.isProcessing = false;
        resolvedAddress.retryCount = 0;
        return updateResolvedAddress.mutateAsync(resolvedAddress);
    };

    useEffect(() => {
        if (resolvedAddressId !== "" && resolvedAddressRetrieved !== undefined) {
            //setResolvedAddress(resolvedAddressRetrieved);
            setMode('VIEW');
        }
    }, [resolvedAddressId, resolvedAddressRetrieved]);


    return (
        <div>
            {resolvedAddressRetrieved !== undefined && (
                <div>
                    <ResolvedAddressDetailCard
                        key={resolvedAddressRetrieved.id.toString()}
                        resolvedAddress={resolvedAddressRetrieved}
                        mode={mode}
                        onRefresh={handleRefresh}
                        onUpdate={handleUpdate}>                   
                        {children}
                    </ResolvedAddressDetailCard>
                </div>
            )}
        </div>
    );
}

export default ResolvedAddressDetail;