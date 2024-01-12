import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import { useParams } from "react-router-dom";
import DataSetSpecificationDetail from "../../components/dataSetSpecifications/dataSetSpecificationDetail";
import BreadCrumbBase from "../../components/bases/layouts/BreadCrumb/BreadCrumbBase";
import { PageLayout } from "../../components/pageLayout";

export const DataSetSpecificationPage = () => {

    const { dataSetId, dataSetSpecificationId } = useParams();

    return <PageLayout>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">

                    <BreadCrumbBase
                        link={"/configuration/dataSet/" + dataSetId + '/'}
                        backLink="DataSet / DataSet Detail"
                        currentLink="DataSet Specification">
                    </BreadCrumbBase>

                    <DataSetSpecificationDetail
                        dataSetSpecificationId={dataSetSpecificationId}
                        dataSetId={dataSetId} />
                </main>
            </Container>
        </section>
    </PageLayout>
}