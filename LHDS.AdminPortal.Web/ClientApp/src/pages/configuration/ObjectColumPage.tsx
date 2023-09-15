import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import { Link, useParams } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft } from "@fortawesome/free-solid-svg-icons";
import ObjectColumnDetail from "../../components/objectColumns/objectColumnDetail";

export const ObjectColumnPage = () => {

    const { dataSetSpecificationId, objectColumnId, specificationObjectId } = useParams();

    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">

                    <Link to={'/configuration/SpecificationObject/' + specificationObjectId 
                        + '/' + dataSetSpecificationId}>

                        <FontAwesomeIcon icon={faChevronLeft} size="1x" />Back to Specifications Object
                    </Link>
                    <ObjectColumnDetail
                        specificationObjectId={specificationObjectId}
                        objectColumnId={objectColumnId}
                        dataSetSpecificationId={dataSetSpecificationId!.toString()}/>
                </main>
            </Container>
        </section>
    </div>
}