import { useParams } from 'react-router-dom';
import IngestionTrackingDetail from "../components/ingestionTracking/ingestionTrackingDetail";
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase";

export const IngestionTrackingPage = () => {

    const { ingestionTrackingId } = useParams();

    return <>
        <section>
            <div className="container-fluid">
                <main role="main">

                    <BreadCrumbBase
                        link="/ingestionTracking"
                        backLink="Ingestion Trackings"
                        currentLink="Ingestion Tracking Detail">
                    </BreadCrumbBase>

                    {ingestionTrackingId &&
                        <IngestionTrackingDetail ingestionTrackingId={ingestionTrackingId} />
                    }
                    <br />
                </main>
            </div>
        </section>
    </>
}