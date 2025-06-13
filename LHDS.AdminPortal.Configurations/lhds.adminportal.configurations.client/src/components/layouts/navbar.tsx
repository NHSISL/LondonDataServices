import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars } from '@fortawesome/free-solid-svg-icons';
import { Button, Container, Navbar } from "react-bootstrap";
import Login from '../securitys/login';
import { useFrontendConfiguration } from '../../hooks/useFrontendConfiguration';

interface NavbarComponentProps {
    toggleSidebar: () => void;
    showMenuButton: boolean;
}

const NavbarComponent: React.FC<NavbarComponentProps> = ({ toggleSidebar, showMenuButton }) => {
    const { configuration } = useFrontendConfiguration();

    return (
        <>
            {configuration?.environment !== "Live" && (
                <div className="environment-tagline"
                    style={{
                        backgroundColor: configuration?.bannerColour || "blue",
                    }}
                >
                    {configuration?.environment?.replace("- ", "")}
                </div>
            )}

            <Navbar className="bg-light p-0" sticky="top" style={{ marginTop: configuration?.environment !== "Live" ? '10px' : '0' }}>
                <Container fluid>
                    {showMenuButton && (
                        <Button onClick={toggleSidebar} variant="outline-dark" className="ms-3">
                            <FontAwesomeIcon icon={faBars} />
                        </Button>
                    )}
                    <Navbar.Brand href="/" className="me-auto ms-3 d-flex align-items-center">
                        <img src="/OneLondon_Logo_OneLondon_Logo_Blue.png" alt="London Data Service logo" height="35" width="108" />
                        <span className="d-none d-md-inline" style={{ marginLeft: "10px" }}>
                            London Data Service Management
                           
                        </span>
                    </Navbar.Brand>
                    <Navbar.Text>
                        <Login />
                    </Navbar.Text>
                </Container>
            </Navbar>
        </>
    );
};

export default NavbarComponent;
