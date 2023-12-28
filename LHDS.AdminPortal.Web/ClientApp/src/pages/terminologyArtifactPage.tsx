import React from "react"
import { PageLayout } from '../components/PageLayout';
import TerminologyArtifactTable from "../components/terminologyArtifacts/terminologyArtifactTable";

export const TerminologyArtifactPage = () => {
    return <PageLayout>
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <TerminologyArtifactTable></TerminologyArtifactTable>
                    </>
                </main>
            </div>
        </section>
    </PageLayout>
}