import React from 'react';
import { Container, Nav, Navbar, NavDropdown } from "react-bootstrap";
import { SecuredComponents, SecuredLink } from './Links';

export const NavigationBar = () => {
    return (
        <>
            <Navbar style={{ backgroundColor: "#005eb8" }}>
                <Container className="nhsuk-width-container" style={{ backgroundColor: "#005eb8" }} >
                    <Navbar.Brand href="#"></Navbar.Brand>
                    <Navbar.Toggle aria-controls="navbarScroll" />
                    <Navbar.Collapse id="navbarScroll">
                        <Nav className="me-auto my-2 my-lg-0"
                            style={{ maxHeight: '100px' }}
                            navbarScroll>
                            <SecuredLink to="/">Home</SecuredLink>
                            <SecuredLink to="/ingestionTracking">Supplier Data</SecuredLink>
                            <SecuredComponents>
                                <NavDropdown title="OptOut" id="optout-dropdown" className="text-white">
                                    <SecuredLink to="/optOutSearch">Search Opt-Out</SecuredLink>
                                    <SecuredLink to="/optOutUpload">Upload Opt-Out</SecuredLink>
                                </NavDropdown>
                            </SecuredComponents>
                            <SecuredComponents>
                                <NavDropdown title="Pds" id="pds-dropdown" className="text-white">
                                    <SecuredLink to="/pds">Pds Audit</SecuredLink>
                                    <SecuredLink to="/pds">Pds Upload</SecuredLink>
                                </NavDropdown>
                            </SecuredComponents>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </>
    );
};