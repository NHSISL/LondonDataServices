import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import { Link, useParams } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft } from "@fortawesome/free-solid-svg-icons";
import DataSetDetail from "../../components/dataSets/dataSetDetail";

export const DataSetPage = () => {

    const { dataSetId } = useParams();

    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <Link to={'/configuration/dataSets'}>
                        <FontAwesomeIcon icon={faChevronLeft} size="1x" />Back to DataSets
                    </Link>
                    <DataSetDetail dataSetId={dataSetId} />
                </main>
            </Container>
        </section>
    </div>
}