import React from "react"
import { PageLayout } from '../components/pageLayout';
import ResolvedAddressTable from "../components/resolvedAddresses/resolvedAddressTable";

export const ResolvedAddressPage = () => {
    return <PageLayout>
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <ResolvedAddressTable></ResolvedAddressTable>
                    </>
                </main>
            </div>
        </section>
    </PageLayout>
}