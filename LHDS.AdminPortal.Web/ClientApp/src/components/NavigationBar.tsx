import React from 'react';
import { Container, Form, Nav, Navbar } from "react-bootstrap";
import {SecuredLink } from './Links';

export const NavigationBar = () => {
    return (
        <>
            <Navbar style={{ backgroundColor: "#005eb8" }}>
                <Container className="nhsuk-width-container" style={{backgroundColor: "#005eb8" }} >
                    <Navbar.Brand href="#"></Navbar.Brand>
                    <Navbar.Toggle aria-controls="navbarScroll" />
                    <Navbar.Collapse id="navbarScroll">
                        <Nav
                            className="me-auto my-2 my-lg-0"
                            style={{ maxHeight: '100px' }}
                            navbarScroll>
                            <SecuredLink to="/">Home</SecuredLink>
                            <SecuredLink to="/ingestionTracking">Supplier Data</SecuredLink>
                        </Nav>
                        <Form className="d-flex">
                        </Form>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </>
    );
};