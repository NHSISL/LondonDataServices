import React, { FunctionComponent } from "react";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import IngestionTrackingRowView from "./ingestionTrackingRowView";

type IngestionTrackingRowProps = {
    ingestionTracking: IngestionTracking;
};

const IngestionTrackingRow: FunctionComponent<IngestionTrackingRowProps> = (props) => {
    const { ingestionTracking } = props;
    return (
        <IngestionTrackingRowView
            key={ingestionTracking.id.toString()}
            ingestionTracking={ingestionTracking} />
    );
};

export default IngestionTrackingRow;