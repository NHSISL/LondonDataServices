import React from "react";
import IngestionTrackingTable from "../components/ingestionTracking/ingestionTrackingTable";
import { PageLayout } from '../components/pageLayout';

export const IngestionTrackingHomepage = () => {
    return <PageLayout>
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <IngestionTrackingTable />
                    </>
                </main>
            </div>
        </section>
    </PageLayout>
}
