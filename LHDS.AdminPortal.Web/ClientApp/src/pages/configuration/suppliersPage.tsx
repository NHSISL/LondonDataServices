import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import SupplierTable from "../../components/suppliers/supplierTable";
import { PageLayout } from "../../components/pageLayout";

export const SuppliersPage = () => {
    return <>
        <PageLayout>
            <section>
                <Container>
                    <main id="maincontent" className="NELTopPadding" role="main">
                        <>
                            <SupplierTable allowedToAdd={true} allowedToEdit={true} allowedToDelete={true} />
                        </>
                    </main>
                </Container>
            </section>
        </PageLayout>
    </>
}