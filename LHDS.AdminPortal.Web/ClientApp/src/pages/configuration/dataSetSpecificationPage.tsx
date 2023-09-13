import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import { Link, useParams } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft } from "@fortawesome/free-solid-svg-icons";
import DataSetSpecificationDetail from "../../components/dataSetSpecifications/dataSetSpecificationDetail";

export const DataSetSpecificationPage = () => {

    const { dataSetId, dataSetSpecificationId } = useParams();

    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <Link to={'/configuration/dataSet/' + dataSetId + '/'}>
                        <FontAwesomeIcon icon={faChevronLeft} size="1x" />Back to DataSet Specifications
                    </Link>
                    <DataSetSpecificationDetail
                        dataSetSpecificationId={dataSetSpecificationId}
                        dataSetId={dataSetId} />
                </main>
            </Container>
        </section>
    </div>
}