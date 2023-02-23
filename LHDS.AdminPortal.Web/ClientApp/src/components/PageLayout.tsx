import React, { ReactElement } from "react";
import { Header } from 'nhsuk-react-components'
import '../styles/base.scss';
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';import { UserProfile } from "./UserProfile";
import { NavigationBar } from "./NavigationBar";
import { BaseFooter } from "./BaseFooter";
;

type PageLayoutParameters = {
    children: ReactElement
}

export const PageLayout = ({ children }: PageLayoutParameters) => {
    return (<>
        <Header orgName="North East London">
            <Header.Container>
                <Header.Logo href="https://northeastlondon.icb.nhs.uk/" />
                <Header.Content>
                    <UserProfile />
                </Header.Content>
            </Header.Container>
            <NavigationBar />
        </Header>

        <main id="maincontent" role="main">
            {children}
        </main>
        <BaseFooter />
    </>)
}

