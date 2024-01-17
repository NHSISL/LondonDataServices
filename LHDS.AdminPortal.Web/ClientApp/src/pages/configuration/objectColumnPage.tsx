import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import { useParams } from "react-router-dom";
import ObjectColumnDetail from "../../components/objectColumns/objectColumnDetail";
import BreadCrumbBase from "../../components/bases/layouts/BreadCrumb/BreadCrumbBase";

export const ObjectColumnPage = () => {

    const { dataSetSpecificationId, objectColumnId, specificationObjectId } = useParams();

    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <BreadCrumbBase
                        link={'/configuration/SpecificationObject/'
                            + specificationObjectId
                            + '/' + dataSetSpecificationId}
                        backLink="DataSet / DataSet Detail / DataSet Specification / Specification Object"
                        currentLink="Object Column">
                    </BreadCrumbBase>

                    <ObjectColumnDetail
                        specificationObjectId={specificationObjectId}
                        objectColumnId={objectColumnId}
                        dataSetSpecificationId={dataSetSpecificationId!.toString()}/>
                </main>
            </Container>
        </section>
    </div>
}