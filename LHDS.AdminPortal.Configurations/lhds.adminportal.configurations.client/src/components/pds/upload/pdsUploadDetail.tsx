import React, { FunctionComponent } from "react";
import PdsUploadDetailCard from "./pdsUploadDetailCard";

interface PdsUploadDetailProps {
    children?: React.ReactNode;
}

const PdsUploadDetail: FunctionComponent<PdsUploadDetailProps> = (props) => {
    const {
        children
    } = props;

    const handleUpload = () => {
        console.log("TODO: if we want to upload");  
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