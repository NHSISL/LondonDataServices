import React from 'react';
import { Container, Form, Nav, Navbar } from "react-bootstrap";
import securityPoints from '../SecurityMatrix';
import { PublicLink, SecuredComponents, SecuredLink } from './Links';

export const NavigationBar = () => {
    return (
        <>
            <Navbar style={{ backgroundColor: "#005eb8" }}>
                <Container style={{ maxWidth: "1040px", backgroundColor: "#005eb8" }} >
                    <Navbar.Brand href="#"></Navbar.Brand>
                    <Navbar.Toggle aria-controls="navbarScroll" />
                    <Navbar.Collapse id="navbarScroll">
                        <Nav
                            className="me-auto my-2 my-lg-0"
                            style={{ maxHeight: '100px' }}
                            navbarScroll>
                            <PublicLink to="/">Home</PublicLink>
                            <PublicLink to="/suppliers">Supplier Data</PublicLink>
                        </Nav>
                        <Form className="d-flex">
                        </Form>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </>
    );
};