import { Guid } from "guid-typescript";
import React, { FunctionComponent} from "react";
import TerminologyArtifactDetailCard from "./terminologyArtifactDetailCard";
import { terminologyArtifactViewService } from "../../services/views/terminologyArtifacts/terminologyArtifactViewService";
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

    const handleRefresh = async (terminologyArtifactView: TerminologyArtifactView) => { }

    const updateDataSet = terminologyArtifactViewService.useUpdateTerminologyArtifact();

    const handleUpdate = async (terminologyArtifact: TerminologyArtifactView) => {
        return updateDataSet.mutateAsync(terminologyArtifact);
    }

    return (
        <div>
            {terminologyArtifactRetrieved !== undefined && (
                <div>
                    <TerminologyArtifactDetailCard
                        key={terminologyArtifactRetrieved.id.toString()}
                        terminologyArtifact={terminologyArtifactRetrieved}
                        onRefresh={handleRefresh}
                        onUpdate={handleUpdate}>                   
                        {children}
                    </TerminologyArtifactDetailCard>
                </div>
            )}
        </div>
    );
}

export default TerminologyArtifactDetail;