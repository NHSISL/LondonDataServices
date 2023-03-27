import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import OptOutDetail from "../components/optOut/optOutDetail";

export const OptOutHomepage = () => {
    return <div>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <OptOutDetail></OptOutDetail>
                    </>
                </main>
            </Container>
        </section>
    </div>
}
