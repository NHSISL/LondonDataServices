import { useIsAuthenticated, useMsal } from '@azure/msal-react';
import { IconProp } from '@fortawesome/fontawesome-svg-core';
import '@fortawesome/fontawesome-svg-core/styles.css'
import { faHome, faDatabase, faCog, faMapMarker, faUserLock, faSitemap } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { ReactElement } from 'react';
import { NavItem, NavLink } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap'
import { Link } from 'react-router-dom';

const iconMapping: Record<string, IconProp | undefined> = {
    faHome: faHome,
    ingestion: faDatabase,
    config: faCog,
    address: faMapMarker,
    optOut: faUserLock,
    terminology: faSitemap
};

type PublicLinkParameters = {
    children: string,
    to: string,
    icon?: string,
    className?: string
}

type SecuredLinkParameters = {
    icon?: string,
    to: string,
    children: string,
    allowedRoles?: Array<string>,
    deniedRoles?: Array<string>
}

type SecuredComponentsParameters = {
    children: ReactElement,
    allowedRoles?: Array<string>,
    deniedRoles?: Array<string>
}

export const PublicLink = ({ to, children, className }: PublicLinkParameters): ReactElement => {

    return (
        <NavItem>
            <LinkContainer to={to} >
                <NavLink className={className}>
                    {children}
                </NavLink>
            </LinkContainer>
        </NavItem>
    )
}

export const SecuredLink = ({ to, children, icon, allowedRoles = [], deniedRoles = [] }: SecuredLinkParameters): ReactElement => {
    const iconProp = icon ? iconMapping[icon] : undefined;

    const { accounts } = useMsal();
    const isAuthenticated = useIsAuthenticated();

    const userRoles = (): Array<string> => {
        if (accounts.length && accounts[0].idTokenClaims && accounts[0].idTokenClaims.roles) {
             return accounts[0].idTokenClaims.roles;
        }

        return []
    }

    const userIsInRole = (roles: Array<string>): Boolean => {
        let found = false;
        roles.forEach(r => {
            if (userRoles().indexOf(r) > -1) {
                found = true;
            }
        });
        return found;
    }

    if (isAuthenticated && userIsInRole(deniedRoles)) {
        return <></>
    }

    if (isAuthenticated && (allowedRoles.length === 0 || userIsInRole(allowedRoles))) {
        return <li className="nav-item">
            <Link to={to} className="nav-link text-white">
                {iconProp && <FontAwesomeIcon icon={iconProp} title="required" className="text-white me-2" />}
                {children}
            </Link>
        </li>
    }

    return <></>;

}

export const SecuredComponents = ({ children, allowedRoles = [], deniedRoles = [] }: SecuredComponentsParameters): ReactElement => {
    const { accounts } = useMsal();
    const isAuthenticated = useIsAuthenticated();

    const userRoles = (): Array<string> => {
        if (accounts.length && accounts[0].idTokenClaims && accounts[0].idTokenClaims.roles) {
            return accounts[0].idTokenClaims.roles;
        }

        return []
    }

    const userIsInRole = (roles: Array<string>): Boolean => {
        let found = false;
        roles.forEach(r => {
            if (userRoles().indexOf(r) > -1) {
                found = true;
            }
        });
        return found;
    }

    if (isAuthenticated && userIsInRole(deniedRoles)) {
        return <></>
    }

    if (isAuthenticated && (allowedRoles.length === 0 || userIsInRole(allowedRoles))) {
        return <>
            {children}
        </>
    }

    return <></>;
}