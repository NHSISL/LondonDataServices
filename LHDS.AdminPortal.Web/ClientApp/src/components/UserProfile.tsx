import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import React, { ReactElement, useState } from "react"
import { Dropdown, DropdownButton, ListGroup, Modal } from "react-bootstrap";
import { loginRequest } from "../authConfig";
import ButtonBase from "./bases/buttons/ButtonBase";
import CardBase from "./bases/components/Card/CardBase";
import CardBaseBody from "./bases/components/Card/CardBase.Body";
import CardBaseContent from "./bases/components/Card/CardBase.Content";
import CardBaseTitle from "./bases/components/Card/CardBase.Title";

export const UserProfile = (): ReactElement => {
    const { instance, accounts } = useMsal();
    const [showModal, setShowModal] = useState(false);

    const closeModal = () => setShowModal(false);
    const openModal = () => setShowModal(true);


    const getName = (): string => {
        if (accounts.length === 0) {
            return ""
        }
        return accounts[0].username;
    }

    return <div>
        <Modal show={showModal} onHide={closeModal} size="lg" centered>
            <Modal.Body>
                <CardBase>
                    <CardBaseBody>
                        <CardBaseTitle>
                            My Profile
                        </CardBaseTitle>

                        <CardBaseContent>
                            <ListGroup>
                                <ListGroup.Item>
                                    <div className="ms-2 me-auto">
                                        <div className="fw-bold">Username / Email</div>
                                        {accounts[0]?.username}
                                    </div>
                                </ListGroup.Item>
                                <ListGroup.Item>
                                    <div className="ms-2 me-auto">
                                        <div className="fw-bold">Name</div>
                                        {accounts[0]?.name}
                                    </div>
                                </ListGroup.Item>
                                {
                                    accounts[0]?.idTokenClaims?.roles?.map((r, i) => <ListGroup.Item key={i}>
                                        <div className="ms-2 me-auto">
                                            <div className="fw-bold">Role</div>
                                            {r}
                                        </div>
                                    </ListGroup.Item>
                                    )
                                }
                            </ListGroup>
                        </CardBaseContent>
                    </CardBaseBody>
                </CardBase>
            </Modal.Body>
            <Modal.Footer>
                <ButtonBase cancel onClick={closeModal}>
                    Close
                </ButtonBase>
            </Modal.Footer>
        </Modal>

        <div style={{ textAlign: "end" }}>
        <AuthenticatedTemplate>
            <DropdownButton 
                title={getName()}>
                <Dropdown.Item onClick={openModal}>My Profile</Dropdown.Item>
                <Dropdown.Item onClick={() => instance.logoutRedirect({ postLogoutRedirectUri: "/" })}>Logout</Dropdown.Item>
            </DropdownButton>
        </AuthenticatedTemplate>
        <UnauthenticatedTemplate>
            <ButtonBase onClick={() => instance.loginRedirect(loginRequest)} add>
                Login
            </ButtonBase>
            </UnauthenticatedTemplate>
            </div>
    </div>;
}