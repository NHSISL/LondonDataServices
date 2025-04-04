import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import OptOutUploadDetail from "../components/optOut/upload/optOutUploadDetail";

export const OptOutUploadPage = () => {
    return <div className="m-3">
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <OptOutUploadDetail></OptOutUploadDetail>
                    </>
                </main>
            </Container>
        </section>
    </div>
}
