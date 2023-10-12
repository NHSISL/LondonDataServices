import React from "react"
import { Link, useParams } from 'react-router-dom';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft } from "@fortawesome/free-solid-svg-icons";
import IngestionTrackingDetail from "../components/ingestionTracking/ingestionTrackingDetail";

export const IngestionTrackingPage = () => {

    const { ingestionTrackingId } = useParams();

    return <div>
        <section >
            <div className="container-fluid">
                <main className="mt-3" role="main">

                    <Link to={'/ingestionTracking'}>
                        <FontAwesomeIcon icon={faChevronLeft} size="1x" className="me-2" />Back to Suppliers Data
                    </Link>

                    {ingestionTrackingId &&
                        <IngestionTrackingDetail ingestionTrackingId={ingestionTrackingId} />
                    }
                    <br />
                </main>
            </div>
        </section>
    </div>
}