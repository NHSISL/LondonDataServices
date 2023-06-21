import React, { FunctionComponent } from "react";
import PdsDetailCard from "./pdsDetailCard";

interface PdsDetailProps {
    children?: React.ReactNode;
}

const PdsDetail: FunctionComponent<PdsDetailProps> = (props) => {
    const {
        children
    } = props;

    return (
        <div>
            <div>
                <PdsDetailCard>
                    {children}
                </PdsDetailCard>
            </div>
        </div>
    );
}

export default PdsDetail;