import { Guid } from "guid-typescript";
import React, { FunctionComponent } from "react";
import { toastError, toastSuccess } from "../../../brokers/toastBroker";
import { Pds } from "../../../models/pds/pds";
import { pdsService } from "../../../services/foundations/pdsService";
import PdsUploadDetailCard from "./pdsUploadDetailCard";

interface PdsUploadDetailProps {
    children?: React.ReactNode;
}

const PdsUploadDetail: FunctionComponent<PdsUploadDetailProps> = (props) => {
    const {
        children
    } = props;

    const addPds = pdsService.useCreatePds();
    const handleUpload = (values: string[]) => {
        values.forEach((value) => {
            let DateNow = new Date();

            const pdsData = {
                id: Guid.create().toString(),
            };

            const pds = new Pds(pdsData);

            return addPds.mutateAsync(pds, {
                onSuccess: () => {
                    console.log(pds);
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
                <PdsUploadDetailCard
                    onUpload={handleUpload}
                >
                    {children}
                </PdsUploadDetailCard>
            </div>
        </div>
    );
}

export default PdsUploadDetail;