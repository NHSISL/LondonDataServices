import { faHome, faUserDoctor } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useState } from 'react';
import { ListGroup } from 'react-bootstrap';
import { useLocation } from 'react-router-dom';
import { SecuredLink } from '../securitys/securedLinks';
import securityPoints from '../../securityMatrix';
import { SecuredComponent } from '../securitys/securedComponents';
import { FeatureDefinitions } from '../../featureDefinitions';
import { FeatureSwitch } from '../accessControls/featureSwitch';

const MenuComponent: React.FC = () => {
    const location = useLocation();
    const [activePath, setActivePath] = useState(location.pathname);

    const handleItemClick = (path: string) => {
        setActivePath(path);
    };

    return (

        <ListGroup variant="flush" className="text-start border-0 mt-5">
            <ListGroup.Item
                className={`bg-dark text-white ${activePath === '/' ? 'active' : ''}`}
                onClick={() => handleItemClick('/')}>
                <FontAwesomeIcon icon={faHome} className="me-2 fa-icon" />
                <SecuredLink to="/home">Home</SecuredLink>
            </ListGroup.Item>

            <FeatureSwitch feature={FeatureDefinitions.IngestionTracking}>
                <SecuredComponent allowedRoles={securityPoints.ingestionTracking.view}>
                    <ListGroup.Item
                        className={`bg-dark text-white ${activePath === '/ingestionTracking' ? 'active' : ''}`}
                        onClick={() => handleItemClick('/ingestionTracking')}>
                        <FontAwesomeIcon icon={faUserDoctor} className="me-2 fa-icon" />
                        <SecuredLink to="/ingestionTracking">Ingestion Tracking</SecuredLink>
                    </ListGroup.Item>
                </SecuredComponent>
            </FeatureSwitch>
            <hr />
            <FeatureSwitch feature={FeatureDefinitions.OptOut}>
                <SecuredComponent allowedRoles={securityPoints.optOut.view}>
                    <ListGroup.Item
                        className={`bg-dark text-white ${activePath === '/optOutSearch' ? 'active' : ''}`}
                        onClick={() => handleItemClick('/optOutSearch')}>
                        <FontAwesomeIcon icon={faUserDoctor} className="me-2 fa-icon" />
                        <SecuredLink to="/optOutSearch">Opt Out</SecuredLink>
                    </ListGroup.Item>
                </SecuredComponent>
            </FeatureSwitch>

            <FeatureSwitch feature={FeatureDefinitions.Pds}>
                <SecuredComponent allowedRoles={securityPoints.pds.view}>
                    <ListGroup.Item
                        className={`bg-dark text-white ${activePath === '/pds' ? 'active' : ''}`}
                        onClick={() => handleItemClick('/pds')}>
                        <FontAwesomeIcon icon={faUserDoctor} className="me-2 fa-icon" />
                        <SecuredLink to="/pds">Patient Demographic Search</SecuredLink>
                    </ListGroup.Item>
                </SecuredComponent>
            </FeatureSwitch>

            <FeatureSwitch feature={FeatureDefinitions.TerminologyArtifact}>
                <SecuredComponent allowedRoles={securityPoints.terminologyArtifact.view}>
                    <ListGroup.Item
                        className={`bg-dark text-white ${activePath === '/terminologyArtifact' ? 'active' : ''}`}
                        onClick={() => handleItemClick('/terminologyArtifact')}>
                        <FontAwesomeIcon icon={faUserDoctor} className="me-2 fa-icon" />
                        <SecuredLink to="/terminologyArtifact">Terminology</SecuredLink>
                    </ListGroup.Item>
                </SecuredComponent>
            </FeatureSwitch>

            <hr />

            {/*<FeatureSwitch feature={FeatureDefinitions.Configuration}>*/}
            {/*    <SecuredComponent allowedRoles={securityPoints.configuration.view}>*/}
            {/*        <ListGroup.Item*/}
            {/*            className={`bg-dark text-white ${activePath === '/configuration' ? 'active' : ''}`}*/}
            {/*            onClick={() => handleItemClick('/configuration')}>*/}
            {/*            <FontAwesomeIcon icon={faUserDoctor} className="me-2 fa-icon" />*/}
            {/*            <SecuredLink to="/configuration">Configuration</SecuredLink>*/}
            {/*        </ListGroup.Item>*/}
            {/*    </SecuredComponent>*/}
            {/*</FeatureSwitch>*/}

            <FeatureSwitch feature={FeatureDefinitions.Configuration}>
                <SecuredComponent allowedRoles={securityPoints.configuration.view}>
                    <ListGroup.Item
                        className={`bg-dark text-white ${activePath === '/configuration/suppliers' ? 'active' : ''}`}
                        onClick={() => handleItemClick('/configuration/suppliers')}>
                        <FontAwesomeIcon icon={faUserDoctor} className="me-2 fa-icon" />
                        <SecuredLink to="/configuration/suppliers">Suppliers</SecuredLink>
                    </ListGroup.Item>
                </SecuredComponent>
            </FeatureSwitch>

            <FeatureSwitch feature={FeatureDefinitions.Configuration}>
                <SecuredComponent allowedRoles={securityPoints.configuration.view}>
                    <ListGroup.Item
                        className={`bg-dark text-white ${activePath === '/subscriberAgreements' ? 'active' : ''}`}
                        onClick={() => handleItemClick('/subscriberAgreements')}>
                        <FontAwesomeIcon icon={faUserDoctor} className="me-2 fa-icon" />
                        <SecuredLink to="/subscriberAgreements">Subscriber Agreements</SecuredLink>
                    </ListGroup.Item>
                </SecuredComponent>
            </FeatureSwitch>

            <hr />

        </ListGroup>
    );
}

export default MenuComponent;