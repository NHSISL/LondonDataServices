import React from "react";
import IngestionTrackingTable from "../components/ingestionTracking/ingestionTrackingTable";

export const IngestionTrackingHomepage = () => {
    return <div className="m-3">
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <IngestionTrackingTable />
                    </>
                </main>
            </div>
        </section>
    </div>
}
