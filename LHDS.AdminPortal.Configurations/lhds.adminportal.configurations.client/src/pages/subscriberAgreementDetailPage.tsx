import React from "react"
import { useParams } from 'react-router-dom';
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase";
import SubscriberAgreementDetail from "../components/subscriberAgreement/subscriberAgreementDetail";

export const SubscriberAgreementDetailPage = () => {

    const {subscriberAgreementId} = useParams();

    return <div className="m-3">
        <section>
            <div className="container-fluid">
                <main role="main">

                    <BreadCrumbBase
                        link="/subscriberAgreements"
                        backLink="Subscriber Agreements"
                        currentLink="Subscriber Agreement Detail">
                    </BreadCrumbBase>

                    {
                        subscriberAgreementId &&
                        <SubscriberAgreementDetail subscriberAgreementId={subscriberAgreementId} />
                    }
                    <br />
                </main>
            </div>
        </section>
    </div>
}