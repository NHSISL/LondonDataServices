import React from "react"
import SubscriberAgreementTable from "../components/subscriberAgreement/subscriberAgreementTable";

export const SubscriberAgreementPage = () => {
    return <>
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <SubscriberAgreementTable />
                    </>
                </main>
            </div>
        </section>
    </>
}