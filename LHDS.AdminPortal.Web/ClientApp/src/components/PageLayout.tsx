import React, { ReactElement } from "react";
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';
import { Header } from 'nhsuk-react-components'

type PageLayoutParameters = {
    children: ReactElement
}

export const PageLayout = ({ children }: PageLayoutParameters) => {
    return (<>
        <Header orgName="North East London">
            <Header.Container>
                <Header.Logo href="https://northeastlondon.icb.nhs.uk/" />
                <Header.Content>
                   
                </Header.Content>
            </Header.Container>
           
        </Header>

        <main id="maincontent" role="main">
            {children}
        </main>
        
    </>)
}

