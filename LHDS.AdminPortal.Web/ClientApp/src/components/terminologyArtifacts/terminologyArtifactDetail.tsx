import { Guid } from "guid-typescript";
import React, { FunctionComponent} from "react";
import TerminologyArtifactDetailCard from "./terminologyArtifactDetailCard";
import { terminologyArtifactViewService } from "../../services/views/TerminologyArtifacts/terminologyArtifactViewService";
import { TerminologyArtifactView } from "../../models/views/components/terminologyArtifacts/terminologyArtifactsView";

interface TerminologyArtifactDetailProps {
    terminologyArtifactId: string;
    children?: React.ReactNode;
}

const TerminologyArtifactDetail: FunctionComponent<TerminologyArtifactDetailProps> = (props) => {
    const {
        terminologyArtifactId,
        children
    } = props;

     const { mappedTerminologyArtifact: terminologyArtifactRetrieved } =
        terminologyArtifactViewService.useGetTerminologyArtifactById(Guid.parse(terminologyArtifactId))

    const handleRefresh = async (terminologyArtifactView: TerminologyArtifactView) => {}

    return (
        <div>
            {terminologyArtifactRetrieved !== undefined && (
                <div>
                    <TerminologyArtifactDetailCard
                        key={terminologyArtifactRetrieved.id.toString()}
                        terminologyArtifact={terminologyArtifactRetrieved}
                        onRefresh={handleRefresh}>
                        {children}
                    </TerminologyArtifactDetailCard>
                </div>
            )}
        </div>
    );
}

export default TerminologyArtifactDetail;