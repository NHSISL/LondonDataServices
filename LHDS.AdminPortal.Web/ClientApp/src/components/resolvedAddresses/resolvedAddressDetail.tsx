import { Guid } from "guid-typescript";
import React, { FunctionComponent} from "react";
import { resolvedAddressViewService } from "../../services/views/resolvedAddresses/resolvedAddressViewService";
import { ResolvedAddressView } from "../../models/views/components/resolvedAddresses/resolvedAddressView";
import ResolvedAddressDetailCard from "./resolvedAddressDetailCard";

interface ResolvedAddressDetailProps {
    resolvedAddressId: string;
    children?: React.ReactNode;
}

const ResolvedAddressDetail: FunctionComponent<ResolvedAddressDetailProps> = (props) => {
    const {
        resolvedAddressId,
        children
    } = props;

     const { mappedResolvedAddress: resolvedAddressRetrieved } =
         resolvedAddressViewService.useGetResolvedAddressById(Guid.parse(resolvedAddressId))

    const handleRefresh = async (resolvedAddressView: ResolvedAddressView) => { }

    const updateResolvedAddress = resolvedAddressViewService.useUpdateResolvedAddress();

    const handleUpdate = async (resolvedAddress: ResolvedAddressView) => {
        return updateResolvedAddress.mutateAsync(resolvedAddress);
    };

    return (
        <div>
            {resolvedAddressRetrieved !== undefined && (
                <div>
                    <ResolvedAddressDetailCard
                        key={resolvedAddressRetrieved.id.toString()}
                        resolvedAddress={resolvedAddressRetrieved}
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