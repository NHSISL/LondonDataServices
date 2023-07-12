import { Guid } from "guid-typescript";
import React, { FunctionComponent } from "react";
import { toastError, toastSuccess } from "../../../brokers/toastBroker";
import { OptOut } from "../../../models/optout/optout";
import { optOutService } from "../../../services/foundations/optoutService";
import OptOutUploadDetailCard from "./optOutUploadDetailCard";

interface OptOutUploadDetailProps {
    children?: React.ReactNode;
}

const OptOutUploadDetail: FunctionComponent<OptOutUploadDetailProps> = (props) => {
    const {
        children
    } = props;

    let onUploadSuccess = false;
    const addOptOut = optOutService.useCreateOptOut();
    const handleUpload = (values: OptOut[]) => {  
        values.forEach((value) => {
            let DateNow = new Date();

            const optOutData = {
                id: Guid.create().toString(),
                uniqueReference: value.uniqueReference ? value.uniqueReference : " ",
                nhsNumber: value.nhsNumber,
                status: value.status ? value.status : 'Unknown',
                cacheTime: DateNow,
                lastSentToMesh: DateNow,
            };

            const optOut = new OptOut(optOutData);

            return addOptOut.mutateAsync(optOut, {
                onSuccess: () => {
                    toastSuccess("Values uploaded successfully.");
                    onUploadSuccess = true;
                },
                onError: (error) => {
                    toastError("Error uploading values.");
                }
            });
        });
    };

    return (
        <div>
            <div>
                <OptOutUploadDetailCard
                    onUpload={handleUpload}
                    onUploadSuccess={onUploadSuccess}>
                    {children}
                </OptOutUploadDetailCard>
            </div>
        </div>
    );
}

export default OptOutUploadDetail;