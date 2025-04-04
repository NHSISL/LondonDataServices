import React, { FunctionComponent } from "react";
import { OptOut } from "../../../models/optout/optout";
import { optOutService } from "../../../services/foundations/optoutService";
import OptOutUploadDetailCard from "./optOutUploadDetailCard";
import { toastError } from "../../../brokers/toastBroker.error";
import { toastSuccess } from "../../../brokers/toastBroker.success";

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
                id: crypto.randomUUID(),
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