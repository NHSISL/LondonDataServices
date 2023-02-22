import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import SupplierTable from "../components/suppliers/supplierTable";

export const SuppliersHomepage = () => {
    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <SupplierTable />
                    </>
                </main>
            </Container>
        </section>
    </div>
}