import React from 'react';
import { Container, Form, Nav, Navbar, NavDropdown } from "react-bootstrap";
import { SecuredComponents, SecuredLink } from './Links';
import securityPoints from '../SecurityMatrix';

export const NavigationBar = () => {
    return (
        <>
            <Navbar style={{ backgroundColor: "#005eb8" }}>
                <Container className="nhsuk-width-container" style={{ backgroundColor: "#005eb8" }}>
                    <Navbar.Brand href="#"></Navbar.Brand>
                    <Navbar.Toggle aria-controls="navbarScroll" />
                    <Navbar.Collapse id="navbarScroll">
                        <Nav className="me-auto my-2 my-lg-0" style={{ maxHeight: '100px' }} navbarScroll>
                            <SecuredLink to="/">Home</SecuredLink>

                            <SecuredComponents allowedRoles={securityPoints.ingestionTracking.view}>
                                <SecuredLink to="/ingestionTracking">Ingestion Tracking</SecuredLink>
                            </SecuredComponents>

                            <SecuredComponents allowedRoles={securityPoints.optOut.view}>
                                <NavDropdown title="OptOut" id="optout-dropdown" className="text-white">
                                    <SecuredLink to="/optOutSearch">Search Opt-Out</SecuredLink>

                                    <SecuredComponents allowedRoles={securityPoints.optOut.upload}>
                                        <SecuredLink to="/optOutUpload">Upload Opt-Out</SecuredLink>
                                    </SecuredComponents>
                                </NavDropdown>

                            </SecuredComponents>
                            <SecuredComponents allowedRoles={securityPoints.pds.view}>
                                <NavDropdown title="Pds" id="pds-dropdown" className="text-white">
                                    <SecuredLink to="/pds">Search Pds Audit</SecuredLink>
                                    <SecuredComponents allowedRoles={securityPoints.pds.upload}>
                                        <SecuredLink to="/pdsUpload">Pds Upload</SecuredLink>
                                    </SecuredComponents>
                                </NavDropdown>
                            </SecuredComponents>

                        </Nav>
                        <Nav className="ms-auto">
                            <Form className="d-flex">
                                <SecuredComponents allowedRoles={securityPoints.configuration.view}>
                                    <SecuredLink to="/configuration">Configuration</SecuredLink>
                                </SecuredComponents>
                            </Form>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>



        </>
    );
};