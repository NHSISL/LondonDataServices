import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import PdsUploadDetail from "../components/pds/upload/pdsUploadDetail";
import { PageLayout } from '../components/PageLayout';

export const PdsUpload = () => {
    return <PageLayout>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <PdsUploadDetail></PdsUploadDetail>
                    </>
                </main>
            </Container>
        </section>
    </PageLayout>
}
