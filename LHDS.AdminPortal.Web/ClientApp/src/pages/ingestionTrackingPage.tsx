import React from "react"
import { Link, useParams } from 'react-router-dom';
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft } from "@fortawesome/free-solid-svg-icons";
import IngestionTrackingDetail from "../components/ingestionTracking/ingestionTrackingDetail";
import AuditTable from "../components/audit/auditTable";

export const IngestionTrackingPage = () => {

    const { ingestionTrackingId } = useParams();

    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">

                    <Link to={'/ingestionTracking'}>
                        <FontAwesomeIcon icon={faChevronLeft} size="1x" />Back to Suppliers Data
                    </Link>
                    {ingestionTrackingId &&
                        <IngestionTrackingDetail ingestionTrackingId={ingestionTrackingId} />
                    }
                    <br />

                    {/*{ingestionTrackingId  && (*/}
                    {/*    <AuditTable ingestionTrackingId={ingestionTrackingId} />*/}
                    {/*)}*/}
                </main>
            </Container>
        </section>
    </div>
}