import React, { useState } from 'react';
import { SecuredComponents, SecuredLink } from './Links';
import securityPoints from '../SecurityMatrix';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCaretDown, faCaretUp } from '@fortawesome/free-solid-svg-icons';

interface SubmenuItem {
    icon: string;
    label: string;
    allowedRoles: string[]; // Replace 'string' with the actual type of 'allowedRoles'
    links: { to: string; label: string }[];
}

interface SubmenuProps {
    items: SubmenuItem;
    allowedRoles: string[]; // Replace 'string' with the actual type of 'allowedRoles'
}

const Submenu: React.FC<SubmenuProps> = ({ items, allowedRoles }) => {
    const [showSubmenu, setShowSubmenu] = useState(false);

    const toggleSubmenu = () => {
        setShowSubmenu(!showSubmenu);
    };

    return (
        <li style={{cursor: "pointer"}} className={`pe-auto ${showSubmenu ? 'submenu-open' : ''}`}>
            <SecuredComponents allowedRoles={allowedRoles}>
                <>
                    <div onClick={toggleSubmenu} className="text-white pe-auto">
                        {items.label} {showSubmenu ? <FontAwesomeIcon icon={faCaretUp} className="ps-2" /> : <FontAwesomeIcon icon={faCaretDown} className="ps-2" />}
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

export const NavigationBar: React.FC = () => {
    const submenuItems: SubmenuItem[] = [
        {
            icon: 'address',
            label: 'Opt-Out',
            allowedRoles: securityPoints.optOut.view,
            links: [
                { to: '/optOutSearch', label: 'Search Opt-Out' },
                securityPoints.optOut.upload && { to: '/optOutUpload', label: 'Upload Opt-Out' },
            ].filter(Boolean) as { to: string; label: string }[],
        },
        {
            icon: 'address',
            label: 'Demographic Search',
            allowedRoles: securityPoints.optOut.view,
            links: [
                { to: '/pds', label: 'Search Pds Audit' },
                securityPoints.pds.upload && { to: '/pdsUpload', label: 'Pds Upload' },
            ].filter(Boolean) as { to: string; label: string }[],
        },
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
                <br />
                <li className="">
                    <SecuredLink icon="faHome" to="/">Home</SecuredLink>
                </li>

                <li className="">
                    <SecuredComponents allowedRoles={securityPoints.ingestionTracking.view}>
                        <SecuredLink icon="ingestion" to="/ingestionTracking">Ingestion Tracking</SecuredLink>
                    </SecuredComponents>
                </li>

                {submenuItems.map((item, index) => (
                    <Submenu key={index} items={item} allowedRoles={item.allowedRoles} />
                ))}
            </ul>
        </>
    );
};
