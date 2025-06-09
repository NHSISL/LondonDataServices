import React, { FunctionComponent, useState } from 'react';
import securityPoints from '../securityMatrix';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCaretDown, faCaretUp } from '@fortawesome/free-solid-svg-icons';
import { SecuredComponents, SecuredLink } from './links';
import { FeatureSwitch } from './accessControl/featureSwitch';
import { FeatureDefinitions } from '../featureDefinitions';

interface SubmenuItem {
    icon: string;
    label: string;
    allowedRoles: string[];
    links: { to: string; label: string; feature?: FeatureDefinitions }[];
    feature: FeatureDefinitions;
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
        <FeatureSwitch feature={items.feature}>
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
                                    <FeatureSwitch feature={items.feature}>
                                        <li key={index} className="nav-item">
                                            <SecuredLink icon="" to={link.to}>{link.label}</SecuredLink>
                                        </li>
                                    </FeatureSwitch>
                                ))}
                            </ul>
                        )}
                    </>
                </SecuredComponents>
            </li>
        </FeatureSwitch>
    );
};

export const NavigationBar: FunctionComponent = () => {
    const submenuItems: SubmenuItem[] = [
        {
            icon: 'config',
            label: 'Configuration',
            allowedRoles: securityPoints.configuration.view,
            feature: FeatureDefinitions.Configuration,
            links: [
                { icon: 'ingestion', to: '/configuration/suppliers', label: 'Suppliers' },
                {
                    icon: 'subscriberAgreement', to: '/subscriberAgreements', label: 'Subscriber Agreements',
                    feature: FeatureDefinitions.SubscriberAgreement
                },
            ].filter(Boolean) as { to: string; label: string }[],
        },
    ];

    return (
        <>
            <ul className="sidebar-nav">
                <li className="mt-4">
                    <SecuredLink icon="faHome" to="/">Home</SecuredLink>
                </li>

                <FeatureSwitch feature={FeatureDefinitions.IngestionTracking}>
                    <li className="">
                        <SecuredComponents allowedRoles={securityPoints.ingestionTracking.view}>
                            <SecuredLink icon="ingestion" to="/ingestionTracking">Ingestion Tracking</SecuredLink>
                        </SecuredComponents>
                    </li>
                </FeatureSwitch>

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

                <FeatureSwitch feature={FeatureDefinitions.TerminologyArtifact}>
                    <li className="">
                        <SecuredComponents allowedRoles={securityPoints.terminologyArtifact.view}>
                            <SecuredLink icon="terminology" to="/terminologyArtifact">Terminology Artifacts</SecuredLink>
                        </SecuredComponents>
                    </li>
                </FeatureSwitch>

                <hr />
                <span style={{ color: 'white', marginLeft: '10px' }}>
                    <strong>Addresses</strong>
                </span>
                <FeatureSwitch feature={FeatureDefinitions.Address}>
                    <li className="">
                        <SecuredComponents allowedRoles={securityPoints.address.view}>
                            <SecuredLink icon="ordanances" to="/address">Post Office Addresses</SecuredLink>
                        </SecuredComponents>
                    </li>
                </FeatureSwitch>

                <FeatureSwitch feature={FeatureDefinitions.ResolvedAddress}>
                    <li className="">
                        <SecuredComponents allowedRoles={securityPoints.resolvedAddress.view}>
                            <SecuredLink icon="resolvedAddress" to="/resolvedAddress">Resolved Addresses</SecuredLink>
                        </SecuredComponents>
                    </li>
                </FeatureSwitch>

                <hr />


                {submenuItems.map((item, index) => (
                    <Submenu key={index} items={item} allowedRoles={item.allowedRoles} />
                ))}
            </ul>
        </>
    );
};
