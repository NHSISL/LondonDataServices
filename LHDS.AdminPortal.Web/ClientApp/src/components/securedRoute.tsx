import { useIsAuthenticated, useMsal } from '@azure/msal-react';
import React, { ReactElement } from 'react';
import { Container } from 'react-bootstrap';
import { WarningCallout } from 'nhsuk-react-components'
import { loginRequest } from '../authConfig';
import ButtonBase from './bases/buttons/ButtonBase';

type SecuredRouteParameters = {
    children: ReactElement,
    allowedRoles?: Array<string>,
    deniedRoles?: Array<string>
}

export const SecuredRoute = ({ children, allowedRoles = [], deniedRoles = [] }: SecuredRouteParameters): ReactElement => {
    const isAuthenticated = useIsAuthenticated();
    const { accounts, instance } = useMsal();


    const userRoles = () => {
        if (accounts.length && accounts[0].idTokenClaims && accounts[0].idTokenClaims.roles) {
            return accounts[0].idTokenClaims.roles;
        }

        return []
    }

    const userIsInRole = (roles: Array<string>) => {
        let found = false;
        roles.forEach(r => {
            if (userRoles().indexOf(r) > -1) {
                found = true;
            }
        });
        return found;
    }

    const NoAccess = () => {
        return <Container className="mt-3">
            <WarningCallout>
                <WarningCallout.Label id="deniedAccessReason">
                    Invalid Access
                </WarningCallout.Label>
                <p>
                    You do not have access to this area of the application, please contact support.
                    <br /><br />

                    {isAuthenticated ? "" : <ButtonBase className="inlineLogin" onClick={() => instance.loginRedirect(loginRequest)} edit>Login</ButtonBase>}
                </p>
            </WarningCallout>
        </Container>
    }

    if (isAuthenticated && userIsInRole(deniedRoles)) {
        return <NoAccess />;
    }

    if (isAuthenticated && (allowedRoles.length === 0 || userIsInRole(allowedRoles))) {
        return <>
            {children}
        </>
    }

    if (isAuthenticated) {
        return <NoAccess />
    }

    return <Container className="mt-3">
        <WarningCallout>
            <WarningCallout.Label visuallyHiddenText="Not Important: " id="deniedAccessReason">
                You are not logged in.
            </WarningCallout.Label>
            <p>
                To access this part of the site, you must first login.<br /><br />
                <ButtonBase className="inlineLoginNotAuth" onClick={() => instance.loginRedirect(loginRequest)} edit>Login</ButtonBase>
            </p>
        </WarningCallout>
    </Container>
}