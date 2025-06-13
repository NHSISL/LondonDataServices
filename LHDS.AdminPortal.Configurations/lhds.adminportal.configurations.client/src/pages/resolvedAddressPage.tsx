import React from "react"
import ResolvedAddressTable from "../components/resolvedAddresses/resolvedAddressTable";

export const ResolvedAddressPage = () => {
    return <div className="m-3">
        <section>
            <div className="container-fluid">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <ResolvedAddressTable></ResolvedAddressTable>
                    </>
                </main>
            </div>
        </section>
    </div>
}