import React from "react"
import { PageLayout } from '../components/pageLayout';
import AddressTable from "../components/addresses/addressTable";

export const AddressPage = () => {
    return <PageLayout>
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <AddressTable></AddressTable>
                    </>
                </main>
            </div>
        </section>
    </PageLayout>
}