import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import DataSetTable from "../../components/dataSets/dataSetTable";

export const DataSetsListPage = () => {
    return <div className="m-3">
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <DataSetTable/>
                    </>
                </main>
            </Container>
        </section>
    </div>
}