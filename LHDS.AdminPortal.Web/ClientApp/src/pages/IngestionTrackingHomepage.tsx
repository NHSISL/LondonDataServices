import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import IngestionTrackingTable from "../components/ingestionTracking/ingestionTrackingTable";



export const IngestionTrackingHomepage = () => {
    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <IngestionTrackingTable />
                    </>
                </main>
            </Container>
        </section>
    </div>
}
