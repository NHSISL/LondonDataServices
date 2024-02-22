import React from "react"
import { useParams } from 'react-router-dom';
import { PageLayout } from '../components/pageLayout';
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase";

export const SubscriberAgreementDetailPage = () => {

    const { subscriberAgreementId } = useParams();

    return <PageLayout>
        <section>
            <div className="container-fluid">
                <main role="main">

                    <BreadCrumbBase
                        link="/subscriberAgreement"
                        backLink="Subscriber Agreements"
                        currentLink="Subscriber Agreement Detail">
                    </BreadCrumbBase>

                    <h1>Subscriber Agreement Detail</h1>
                    <br />
                </main>
            </div>
        </section>
    </PageLayout>
}