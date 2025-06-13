import { useParams } from 'react-router-dom';
import BreadCrumbBase from "../components/bases/layouts/BreadCrumb/BreadCrumbBase";
import ResolvedAddressDetail from "../components/resolvedAddresses/resolvedAddressDetail";

export const ResolvedAddressDetailPage = () => {

    const { resolvedAddressId } = useParams();

    return <div className="m-3">
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
    </div>
}