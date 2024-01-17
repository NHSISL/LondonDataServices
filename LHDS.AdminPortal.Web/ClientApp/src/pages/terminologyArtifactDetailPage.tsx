import React from "react"
import { useParams } from 'react-router-dom';
import TerminologyArtifactDetail from "../components/terminologyArtifacts/terminologyArtifactDetail";
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase";
import { PageLayout } from '../components/pageLayout';

export const TerminologyArtifactDetailPage = () => {

    const { terminologyArtifactId } = useParams();

    return <PageLayout>
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
    </PageLayout>
}