import React from "react"
import { useParams } from 'react-router-dom';
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase";
import AddressDetail from "../components/addresses/addressDetail";

export const AddressDetailPage = () => {

    const { addressId } = useParams();

    return <div className="m-3">
        <section>
            <div className="container-fluid">
                <main role="main">

                    <BreadCrumbBase
                        link="/address"
                        backLink="Address"
                        currentLink="Address Detail">
                    </BreadCrumbBase>

                    {
                        addressId &&
                        <AddressDetail addressId={addressId} />
                    }
                    
                    <br />
                </main>
            </div>
        </section>
    </div>
}