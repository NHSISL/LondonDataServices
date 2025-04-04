import React from "react"
import TerminologyArtifactDetail from "../components/terminologyArtifacts/terminologyArtifactDetail";
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase";
import { useParams } from "react-router-dom";

export const TerminologyArtifactDetailPage = () => {

    const { terminologyArtifactId } = useParams();

    return <div className="m-3">
        <section>
            <div className="container-fluid">
                <main role="main">

                    <BreadCrumbBase
                        link="/terminologyArtifact"
                        backLink="Terminlogy Artifacts"
                        currentLink="Terminology Artifacts Detail">
                    </BreadCrumbBase>

                    {
                        terminologyArtifactId &&
                            <TerminologyArtifactDetail terminologyArtifactId={terminologyArtifactId} />
                    }
                    
                    <br />
                </main>
            </div>
        </section>
    </div>
}