import React, { FunctionComponent } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import TerminologyArtifactDetailCardView from "./terminologyArtifactDetailCardView";
import { TerminologyArtifactView } from "../../models/views/components/terminologyArtifacts/terminologyArtifactsView";

interface TerminologyArtifactDetailCardProps {
    terminologyArtifact: TerminologyArtifactView;
    children?: React.ReactNode;
    onRefresh: (terminologyArtifact: TerminologyArtifactView) => void;
    onUpdate: (terminologyArtifact: TerminologyArtifactView,) => void;
}

const TerminologyArtifactDetailCard: FunctionComponent<TerminologyArtifactDetailCardProps> = (props) => {
    const {
        terminologyArtifact,
        children,
        onRefresh,
        onUpdate
    } = props;

    const handlRefresh = async (terminologyArtifact: TerminologyArtifactView) => {
        await onRefresh(terminologyArtifact);
    }

    const handleUpdate = async (terminologyArtifact: TerminologyArtifactView) => {
        await onUpdate(terminologyArtifact);
    };

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Terminology Artifact
                    </CardBaseTitle>
                    <CardBaseContent>
                        <TerminologyArtifactDetailCardView
                            terminologyArtifact={terminologyArtifact}
                            onRefresh={handlRefresh}
                            onUpdate={handleUpdate}
                        />

                        {children !== undefined && (
                            <>
                                <br />
                                {children}
                            </>
                        )}
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
}

export default TerminologyArtifactDetailCard;