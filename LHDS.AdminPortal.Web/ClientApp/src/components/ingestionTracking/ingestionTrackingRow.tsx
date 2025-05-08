import React, { FunctionComponent } from "react";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import IngestionTrackingRowView from "./ingestionTrackingRowView";

type IngestionTrackingRowProps = {
    onReDecrypted: (ingestionTracking: IngestionTracking) => void;
    ingestionTracking: IngestionTracking;
};

const IngestionTrackingRow: FunctionComponent<IngestionTrackingRowProps> = (props) => {
    const {
        onReDecrypted,
        ingestionTracking
    } = props;

    const handleReDecrypt = async (ingestionTracking: IngestionTracking) => {
        await onReDecrypted(ingestionTracking);
    };

    return (
        <IngestionTrackingRowView
            key={ingestionTracking.id.toString()}
            ingestionTracking={ingestionTracking}
            onReDecrypted={handleReDecrypt}
        />
    );
};

export default IngestionTrackingRow;