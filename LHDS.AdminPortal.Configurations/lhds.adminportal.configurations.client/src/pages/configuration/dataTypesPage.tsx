import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import DataTypeTable from "../../components/dataTypes/dataTypeTable";

export const DataTypesPage = () => {
    return <div className="m-3">
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <DataTypeTable allowedToAdd={true} allowedToEdit={true} allowedToDelete={true} />
                    </>
                </main>
            </Container>
        </section>
    </div>
}