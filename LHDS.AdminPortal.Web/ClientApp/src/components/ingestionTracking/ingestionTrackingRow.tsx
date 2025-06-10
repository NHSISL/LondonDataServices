import React, { FunctionComponent } from "react";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import IngestionTrackingRowView from "./ingestionTrackingRowView";

type IngestionTrackingRowProps = {
    onReDecrypted: (ingestionTracking: IngestionTracking) => void;
    ingestionTracking: IngestionTracking;
    onBatchClick: (batch: string) => void;
};

const IngestionTrackingRow: FunctionComponent<IngestionTrackingRowProps> = (props) => {
    const {
        onReDecrypted,
        ingestionTracking,
        onBatchClick
    } = props;

    const handleReDecrypt = async (ingestionTracking: IngestionTracking) => {
        await onReDecrypted(ingestionTracking);
    };

    return (
        <IngestionTrackingRowView
            key={ingestionTracking.id.toString()}
            ingestionTracking={ingestionTracking}
            onReDecrypted={handleReDecrypt}
            onBatchClick={onBatchClick}
        />
    );
};

export default IngestionTrackingRow;