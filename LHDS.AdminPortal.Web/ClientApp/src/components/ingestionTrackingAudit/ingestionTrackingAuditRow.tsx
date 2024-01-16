import React, { FunctionComponent } from "react";
import IngestionTrackingAuditRowView from "./ingestionTrackingAuditRowView";
import { IngestionTrackingAuditView } from "../../models/views/components/ingestionTrackingAudit/ingestionTrackingAuditView";

type IngestionTrackingAuditRowProps = {
    ingestionTrackingAudit: IngestionTrackingAuditView;
}

const IngestionTrackingAuditRow: FunctionComponent<IngestionTrackingAuditRowProps> = (props) => {
    const {
        ingestionTrackingAudit,
    } = props;

    return (
        <IngestionTrackingAuditRowView
            key={ingestionTrackingAudit.id.toString()}
            ingestionTrackingAudit={ ingestionTrackingAudit } />
    );
}

export default IngestionTrackingAuditRow;