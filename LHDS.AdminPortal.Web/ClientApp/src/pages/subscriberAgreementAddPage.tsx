import React from "react"
import { PageLayout } from '../components/pageLayout';
import SubscriberAgreementAdd from "../components/subscriberAgreement/subscriberAgreementAdd";
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase";

export const SubscriberAgreementAddPage = () => {
    return <PageLayout>
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <BreadCrumbBase
                            link="/subscriberAgreements"
                            backLink="Subscriber Agreements"
                            currentLink="Subscriber Agreement Add">
                        </BreadCrumbBase>

                        <SubscriberAgreementAdd />
                    </>
                </main>
            </div>
        </section>
    </PageLayout>
}