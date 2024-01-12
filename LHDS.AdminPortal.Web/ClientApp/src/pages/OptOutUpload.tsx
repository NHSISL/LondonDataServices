import React from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Container } from 'nhsuk-react-components'
import OptOutUploadDetail from "../components/optOut/upload/optOutUploadDetail";
import { PageLayout } from '../components/pageLayout';

export const OptOutUpload = () => {
    return <PageLayout>
        <section >
            <Container>
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <OptOutUploadDetail></OptOutUploadDetail>
                    </>
                </main>
            </Container>
        </section>
    </PageLayout>
}
