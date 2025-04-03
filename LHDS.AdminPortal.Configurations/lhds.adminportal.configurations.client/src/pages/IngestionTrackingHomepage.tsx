import React from "react";
import IngestionTrackingTable from "../components/ingestionTracking/ingestionTrackingTable";

export const IngestionTrackingHomepage = () => {
    return <>
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <IngestionTrackingTable />
                    </>
                </main>
            </div>
        </section>
    </>
}
