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
                            <SecuredLink to="/ingestionTracking">Ingestion Tracking</SecuredLink>
                            <SecuredComponents>
                                <NavDropdown title="OptOut" id="optout-dropdown" className="text-white">
                                    <SecuredLink to="/optOutSearch">Search Opt-Out</SecuredLink>
                                    <SecuredLink to="/optOutUpload">Upload Opt-Out</SecuredLink>
                                </NavDropdown>
                            </SecuredComponents>
                            <SecuredComponents allowedRoles={securityPoints.pdsNavigation.view}>
                                <NavDropdown title="Pds" id="pds-dropdown" className="text-white">
                                    <SecuredLink to="/pds">Pds Audit</SecuredLink>
                                    <SecuredLink to="/pdsUpload">Pds Upload</SecuredLink>
                                </NavDropdown>
                            </SecuredComponents>
                        </Nav>
                        <Nav className="ms-auto">
                            <Form className="d-flex">
                                <SecuredComponents>
                                    <SecuredLink to="/configuration">Admin Configuration</SecuredLink>
                                </SecuredComponents>
                            </Form>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>



        </>
    );
};