import React, { FunctionComponent, useState } from 'react';
import securityPoints from '../securityMatrix';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCaretDown, faCaretUp, faUserLock } from '@fortawesome/free-solid-svg-icons';
import { SecuredComponents, SecuredLink } from './links';

interface SubmenuItem {
    icon: string;
    label: string;
    allowedRoles: string[];
    links: { to: string; label: string }[];
}

interface SubmenuProps {
    items: SubmenuItem;
    allowedRoles: string[];
}

const Submenu: FunctionComponent<SubmenuProps> = (props) => {
    const {
        items,
        allowedRoles
    } = props;

    const [showSubmenu, setShowSubmenu] = useState(false);

    const toggleSubmenu = () => {
        setShowSubmenu(!showSubmenu);
    };

    return (
        <li style={{ cursor: "pointer" }} className={`pe-auto ${showSubmenu ? 'submenu-open' : ''}`}>
            <SecuredComponents allowedRoles={allowedRoles}>
                <>
                    <div onClick={toggleSubmenu} className="text-white pe-auto">
                        {items.label} {showSubmenu
                            ? <FontAwesomeIcon icon={faCaretUp} className="ps-2" />
                            : <FontAwesomeIcon icon={faCaretDown} className="ps-2" />}
                    </div>
                    {showSubmenu && (
                        <ul className="">
                            {items.links.map((link, index) => (
                                <li key={index} className="nav-item">
                                    <SecuredLink icon="" to={link.to}>{link.label}</SecuredLink>
                                </li>
                            ))}
                        </ul>
                    )}
                </>
            </SecuredComponents>
        </li>
    );
};

export const NavigationBar: FunctionComponent = () => {
    const submenuItems: SubmenuItem[] = [
        {
            icon: 'config',
            label: 'Configuration',
            allowedRoles: securityPoints.configuration.view,
            links: [
                { icon: 'ingestion', to: '/configuration/suppliers', label: 'Suppliers' },
                { icon: 'ingestion', to: '/configuration/dataTypes', label: 'Data Types' },
                { icon: 'ingestion', to: '/configuration/dataSets', label: 'Data Sets' },
            ].filter(Boolean) as { to: string; label: string }[],
        },
    ];

    return (
        <>
            <ul className="sidebar-nav">

                <li className="mt-4">
                    <SecuredLink icon="faHome" to="/">Home</SecuredLink>
                </li>

                <li className="">
                    <SecuredComponents allowedRoles={securityPoints.ingestionTracking.view}>
                        <SecuredLink icon="ingestion" to="/ingestionTracking">Ingestion Tracking</SecuredLink>
                    </SecuredComponents>
                </li>

                <li className="">
                    <SecuredComponents allowedRoles={securityPoints.optOut.view}>
                        <SecuredLink icon="optOut" to="/optOutSearch">Patient Opt Out</SecuredLink>
                    </SecuredComponents>
                </li>

                <li className="">
                    <SecuredComponents allowedRoles={securityPoints.pds.view}>
                        <SecuredLink icon="address" to="/pds">Patient Demographic Search</SecuredLink>
                    </SecuredComponents>
                </li>

                <li className="">
                    <SecuredComponents allowedRoles={securityPoints.terminologyArtifact.view}>
                        <SecuredLink icon="terminology" to="/terminologyArtifact">Terminology Artifacts</SecuredLink>
                    </SecuredComponents>
                </li>

                {submenuItems.map((item, index) => (
                    <Submenu key={index} items={item} allowedRoles={item.allowedRoles} />
                ))}

            </ul>
        </>
    );
};
