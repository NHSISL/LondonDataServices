import React from "react"
import { useParams } from 'react-router-dom';
import { PageLayout } from '../components/pageLayout';
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase";
import ResolvedAddressDetail from "../components/resolvedAddresses/resolvedAddressDetail";

export const ResolvedAddressDetailPage = () => {

    const { resolvedAddressId } = useParams();

    return <PageLayout>
        <section>
            <div className="container-fluid">
                <main role="main">

                    <BreadCrumbBase
                        link="/resolvedAddress"
                        backLink="ResolvedAddress"
                        currentLink="Resolved Address Detail">
                    </BreadCrumbBase>

                    {
                        resolvedAddressId &&
                        <ResolvedAddressDetail resolvedAddressId={resolvedAddressId} onPickAlternateAddress={() => { }} />
                    }
                    
                    <br />
                </main>
            </div>
        </section>
    </PageLayout>
}