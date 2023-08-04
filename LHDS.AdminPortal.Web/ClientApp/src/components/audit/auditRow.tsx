import React, { FunctionComponent } from "react";
import AuditRowView from "./auditRowView";
import { AuditView } from "../../models/views/components/audit/auditView";

type AuditRowProps = {
    audit: AuditView;
}

const AuditRow: FunctionComponent<AuditRowProps> = (props) => {
    const {
        audit,
       
    } = props;

    return (
        <AuditRowView
            key={audit.id.toString()}
            audit={ audit } />
    );
}
export default AuditRow;