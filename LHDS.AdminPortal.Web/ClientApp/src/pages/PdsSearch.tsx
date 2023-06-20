import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import PdsDetail from "../components/pds/search/pdsDetail";

export const PdsSearch = () => {
    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <PdsDetail></PdsDetail>
                    </>
                </main>
            </Container>
        </section>
    </div>
}
