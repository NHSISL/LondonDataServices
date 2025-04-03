import React, { FunctionComponent } from "react";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import IngestionTrackingRowView from "./ingestionTrackingRowView";

type IngestionTrackingRowProps = {
    onEncryptedDownload: (ingestionTracking: IngestionTracking) => void;
    onReDecrypted: (ingestionTracking: IngestionTracking) => void;
    ingestionTracking: IngestionTracking;
};

const IngestionTrackingRow: FunctionComponent<IngestionTrackingRowProps> = (props) => {
    const {
        onEncryptedDownload,
        onReDecrypted,
        ingestionTracking
    } = props;

    const handleEncryptedDownload = async (ingestionTracking: IngestionTracking) => {
        await onEncryptedDownload(ingestionTracking);
    };

    const handleReDecrypt = async (ingestionTracking: IngestionTracking) => {
        await onReDecrypted(ingestionTracking);
    };

    return (
        <IngestionTrackingRowView
            key={ingestionTracking.id.toString()}
            ingestionTracking={ingestionTracking}
            onEncryptedDownload={handleEncryptedDownload}
            onReDecrypted={handleReDecrypt}
        />
    );
};

export default IngestionTrackingRow;