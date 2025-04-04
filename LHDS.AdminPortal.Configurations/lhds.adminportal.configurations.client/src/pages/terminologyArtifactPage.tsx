import React from "react"
import TerminologyArtifactTable from "../components/terminologyArtifacts/terminologyArtifactTable";

export const TerminologyArtifactPage = () => {
    return <div className="m-3">
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <TerminologyArtifactTable></TerminologyArtifactTable>
                    </>
                </main>
            </div>
        </section>
    </div>
}