import React, { FunctionComponent } from "react";
import { TerminologyArtifact } from "../../models/terminologyArtifacts/terminologyArtifact";
import TerminologyArtifactRowView from "./terminologyArtifactRowView";

type TerminologyArtifactRowProps = {
    terminologyArtifact: TerminologyArtifact;
};

const TerminologyArtifactRow: FunctionComponent<TerminologyArtifactRowProps> = (props) => {
    const {
        terminologyArtifact
    } = props;

    return (
        <TerminologyArtifactRowView
            key={terminologyArtifact.id.toString()}
            terminologyArtifact={terminologyArtifact}
        />
    );
};

export default TerminologyArtifactRow;