import React from 'react';
import { Container, Row, Col, Card } from 'nhsuk-react-components';
import 'nhsuk-frontend/dist/nhsuk.min';
import 'nhsuk-frontend/packages/polyfills';
import { PublicLink, SecuredComponents } from '../components/Links';
import { useIsAuthenticated, useMsal } from '@azure/msal-react';
import { loginRequest } from '../authConfig';
import { Button } from 'react-bootstrap';
import { FeatureSwitch } from '../components/accessControl/FeatureSwitch';
import { FeatureDefinitions } from '../featureDefinitions';
import securityPoints from '../SecurityMatrix';

export const Home = () => {
    const isAuthenticated = useIsAuthenticated();
    const { instance } = useMsal();
    return (
        <>
            <section className="nhsuk-hero">
                <Container>
                    <Row className="nhsuk-grid-row">
                        <Col width="full">
                            <div className="nhsuk-hero__wrapper app-hero__wrapper">
                                <h1 className="nhsuk-u-margin-bottom-4">London Health Data Services</h1>
                                <span className=" nhsuk-u-margin-bottom-1">
                                    <p className="nhsuk-body-l nhsuk-u-margin-bottom-1">Admin Portal</p>
                                </span>
                            </div>
                        </Col>
                        <Col width="one-third"></Col>
                    </Row>
                </Container>
            </section>

            <section>
                <Container className="NELTopPadding">
                    {isAuthenticated ? (
                        <div>
                            <Card.Group>
                                <FeatureSwitch feature={FeatureDefinitions.IngestionTracking}>
                                    <SecuredComponents allowedRoles={securityPoints.ingestionTracking.view}>
                                        <Card.GroupItem width="one-half">
                                            <Card clickable>
                                                <Card.Content>
                                                    <Card.Heading className="nhsuk-card__heading nhsuk-heading-m">
                                                        <PublicLink to="/ingestionTracking">Ingestion Tracking</PublicLink>
                                                    </Card.Heading>
                                                    <Card.Description>
                                                        View Ingestion Data.
                                                    </Card.Description>
                                                </Card.Content>
                                            </Card>
                                        </Card.GroupItem>
                                    </SecuredComponents>

                                    <SecuredComponents allowedRoles={securityPoints.optOut.view}>
                                        <Card.GroupItem width="one-half">
                                            <Card clickable>
                                                <Card.Content>
                                                    <Card.Heading className="nhsuk-card__heading nhsuk-heading-m">
                                                        <PublicLink to="/optOutSearch">Search OptOut</PublicLink>
                                                    </Card.Heading>
                                                    <Card.Description>
                                                        Search Opt Out.
                                                    </Card.Description>
                                                </Card.Content>
                                            </Card>
                                        </Card.GroupItem>
                                    </SecuredComponents>


                                    <SecuredComponents allowedRoles={securityPoints.pds.view}>
                                        <Card.GroupItem width="one-half">
                                            <Card clickable>
                                                <Card.Content>
                                                    <Card.Heading className="nhsuk-card__heading nhsuk-heading-m">
                                                        <PublicLink to="/pds">Search PDS</PublicLink>
                                                    </Card.Heading>
                                                    <Card.Description>
                                                        Search Pds.
                                                    </Card.Description>
                                                </Card.Content>
                                            </Card>
                                        </Card.GroupItem>
                                    </SecuredComponents>
                                </FeatureSwitch>
                            </Card.Group>
                        </div>
                    ) : (
                        <Card>
                            <Card.Content>
                                <Card.Heading className="nhsuk-heading-m">
                                        <Card.Link href="#" onClick={() => instance.loginRedirect(loginRequest)}>
                                            Login to continue.
                                        </Card.Link>
                                </Card.Heading>
                                <Card.Description>
                                    To unlock all the features of this system,
                                        please

                                        <Button
                                            variant="link"
                                            className="linkCustom"
                                            onClick={() => instance.loginRedirect(loginRequest)}>
                                            Login
                                        </Button>.

                                    For access requests, please contact your Manager.
                                </Card.Description>
                            </Card.Content>
                        </Card>
                    )}
                </Container>
            </section>
        </>
    );
};
