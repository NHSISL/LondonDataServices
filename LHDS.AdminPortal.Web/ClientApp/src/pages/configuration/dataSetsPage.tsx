import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import DataSetTable from "../../components/dataSets/dataSetTable";

export const DataSetsPage = () => {
    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <DataSetTable allowedToAdd={true} allowedToEdit={true} allowedToDelete={true} />
                    </>
                </main>
            </Container>
        </section>
    </div>
}