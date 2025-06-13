import React, { FunctionComponent, useEffect, useState} from "react";
import { resolvedAddressViewService } from "../../services/views/resolvedAddresses/resolvedAddressViewService";
import { ResolvedAddressView } from "../../models/views/components/resolvedAddresses/resolvedAddressView";
import ResolvedAddressDetailCard from "./resolvedAddressDetailCard";
import { toastSuccess } from "../../brokers/toastBroker.success";
import { toastError } from "../../brokers/toastBroker.error";

interface ResolvedAddressDetailProps {
    resolvedAddressId: string;
    onPickAlternateAddress: (alternateAddress: string) => void;
    children?: React.ReactNode;
}

const ResolvedAddressDetail: FunctionComponent<ResolvedAddressDetailProps> = (props) => {
    const {
        resolvedAddressId,
        onPickAlternateAddress,
        children
    } = props;

    const [mode, setMode] = useState<string>('VIEW');

    const { mappedResolvedAddress: resolvedAddressRetrieved } =
        resolvedAddressViewService.useGetResolvedAddressById(resolvedAddressId);

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

        return updateResolvedAddress.mutateAsync(resolvedAddress)
            .then(() => {
                toastSuccess("Alternate Address Saved")
            })
            .catch(e => {
                toastError("error")
            });
    };

    useEffect(() => {
        if (resolvedAddressId !== "" && resolvedAddressRetrieved !== undefined) {
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