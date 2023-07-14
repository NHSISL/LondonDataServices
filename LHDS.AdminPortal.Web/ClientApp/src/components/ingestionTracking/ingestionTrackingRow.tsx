import React, { FunctionComponent } from "react";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import IngestionTrackingRowView from "./ingestionTrackingRowView";

type IngestionTrackingRowProps = {
    onRelanding: (ingestionTracking: IngestionTracking) => void;
    onRedecrypt: (ingestionTracking: IngestionTracking) => void;
    onEncryptedDownload: (ingestionTracking: IngestionTracking) => void;
    onDecryptedDownload: (ingestionTracking: IngestionTracking) => void;
    ingestionTracking: IngestionTracking;
};

const IngestionTrackingRow: FunctionComponent<IngestionTrackingRowProps> = (props) => {
    const {
        onRelanding,
        onRedecrypt,
        onEncryptedDownload,
        onDecryptedDownload,
        ingestionTracking
    } = props;

    const handleRelanding = async (ingestionTracking: IngestionTracking) => {
        await onRelanding(ingestionTracking);
    };

    const handleRedecrypt = async (ingestionTracking: IngestionTracking) => {
        await onRedecrypt(ingestionTracking);
    };

    const handleEncryptedDownload = async (ingestionTracking: IngestionTracking) => {
        await onEncryptedDownload(ingestionTracking);
    };

    const handleDecryptedDownload = async (ingestionTracking: IngestionTracking) => {
        await onDecryptedDownload(ingestionTracking);
    };

    return (
        <IngestionTrackingRowView
            key={ingestionTracking.id.toString()}
            ingestionTracking={ingestionTracking}
            onRelanding={handleRelanding}
            onRedecrypt={handleRedecrypt}
            onEncryptedDownload={handleEncryptedDownload}
            onDecryptedDownload={handleDecryptedDownload}
        />
    );
};

export default IngestionTrackingRow;