import React from "react"
import { PageLayout } from '../components/pageLayout';
import SubscriberAgreementAdd from "../components/subscriberAgreement/subscriberAgreementAdd";

export const SubscriberAgreementAddPage = () => {
    return <PageLayout>
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <SubscriberAgreementAdd />
                    </>
                </main>
            </div>
        </section>
    </PageLayout>
}