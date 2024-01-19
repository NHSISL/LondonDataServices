import React from "react";
import { useParams } from "react-router-dom";
import DataSetDetail from "../../components/dataSets/dataSetDetail";
import { PageLayout } from "../../components/pageLayout";
import BreadCrumbBase from "../../components/bases/layouts/BreadCrumb/BreadCrumbBase";

export const DataSetPage = () => {

    const { dataSetId } = useParams();

    return <PageLayout>
        <section >
            <div className="container-fluid">
                <main id="maincontent" className="NELTopPadding" role="main">

                    <BreadCrumbBase
                        link="/configuration/dataSets"
                        backLink="DataSets"
                        currentLink="DataSet Detail">
                    </BreadCrumbBase>

                    <DataSetDetail dataSetId={dataSetId} />
                </main>
            </div>
        </section>
    </PageLayout>
}