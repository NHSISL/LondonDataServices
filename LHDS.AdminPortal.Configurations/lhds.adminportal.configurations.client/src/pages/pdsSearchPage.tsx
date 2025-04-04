import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import PdsTable from "../components/pds/search/pdsTable";

export const PdsSearchPage = () => {
    return <>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <PdsTable></PdsTable>
                    </>
                </main>
            </Container>
        </section>
    </>
}
