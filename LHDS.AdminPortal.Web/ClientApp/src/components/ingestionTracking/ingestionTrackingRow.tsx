import React, { FunctionComponent } from "react";
import Badge from "react-bootstrap/esm/Badge";
import { Link } from "react-router-dom";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import ButtonBase from "../bases/buttons/ButtonBase";
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