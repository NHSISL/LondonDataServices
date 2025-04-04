import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import SpecificationObjectDetail from "../../components/specificationObjects/specificationObjectDetail";
import BreadCrumbBase from "../../components/bases/layouts/BreadCrumb/BreadCrumbBase";
import { useParams } from "react-router-dom";
import { PageLayout } from "../../components/pageLayout";

export const SpecificationObjectPage = () => {

    const { specificationObjectId, dataSetSpecificationId, dataSetId } = useParams();

    return <PageLayout>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">

                    <BreadCrumbBase
                        link={'/configuration/dataSetSpecification/' + specificationObjectId + '/' + dataSetSpecificationId }
                        backLink="DataSet / DataSet Detail / DataSet Specification"
                        currentLink="Specification Object">
                    </BreadCrumbBase>

                    <SpecificationObjectDetail
                        dataSetSpecificationId={dataSetSpecificationId}
                        specificationObjectId={specificationObjectId}
                        dataSetId={dataSetId!}
                    />
                </main>
            </Container>
        </section>
    </PageLayout>
}