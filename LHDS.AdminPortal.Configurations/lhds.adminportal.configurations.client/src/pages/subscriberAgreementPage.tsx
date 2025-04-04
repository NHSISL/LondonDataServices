import React from "react"
import SubscriberAgreementTable from "../components/subscriberAgreement/subscriberAgreementTable";

export const SubscriberAgreementPage = () => {
    return <div className="m-3">
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <SubscriberAgreementTable />
                    </>
                </main>
            </div>
        </section>
    </div>
}