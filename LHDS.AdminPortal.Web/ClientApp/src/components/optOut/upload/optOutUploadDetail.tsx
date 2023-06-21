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

    const addOptOut = optOutService.useCreateOptOut();
    const handleUpload = (values: string[]) => {
        values.forEach((value) => {
            let DateNow = new Date();

            const optOutData = {
                id: Guid.create().toString(),
                nhsNumber: value,
                optOutStatus: 'Unknown',
                cacheTime: DateNow,
                lastSentToMesh: DateNow,
            };

            const optOut = new OptOut(optOutData);

            console.log(Guid.create());

            console.log(optOut);

            return addOptOut.mutateAsync(optOut, {
                onSuccess: () => {
                    console.log(optOut);
                    toastSuccess("Values uploaded successfully.");
                },
                onError: (error) => {
                    console.log(error);
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
                >
                    {children}
                </OptOutUploadDetailCard>
            </div>
        </div>
    );
}

export default OptOutUploadDetail;