import React from "react"
import AddressTable from "../components/addresses/addressTable";

export const AddressPage = () => {
    return <div className="m-3">
        <section>
            <div className="container-fluis">
                <main id="maincontent" className="NELTopPadding" role="main">
                    <>
                        <AddressTable></AddressTable>
                    </>
                </main>
            </div>
        </section>
    </div>
}