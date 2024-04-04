import React from "react"
import { PageLayout } from '../components/pageLayout';
import SubscriberAgreementTable from "../components/subscriberAgreement/subscriberAgreementTable";

export const SubscriberAgreementPage = () => {
    return <PageLayout>
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <SubscriberAgreementTable />
                    </>
                </main>
            </div>
        </section>
    </PageLayout>
}