import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import PdsUploadDetail from "../components/pds/upload/pdsUploadDetail";

export const PdsUploadPage = () => {
    return <div className="m-3">
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <PdsUploadDetail></PdsUploadDetail>
                    </>
                </main>
            </Container>
        </section>
    </div>
}
