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

        </ListGroup>
    );
}

export default MenuComponent;