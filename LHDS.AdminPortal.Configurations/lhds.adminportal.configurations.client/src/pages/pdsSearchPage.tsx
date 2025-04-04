import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import PdsTable from "../components/pds/search/pdsTable";

export const PdsSearchPage = () => {
    return <div className="m-3">
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <PdsTable></PdsTable>
                    </>
                </main>
            </Container>
        </section>
    </div>
}
