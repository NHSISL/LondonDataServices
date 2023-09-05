import React from 'react';
import { Container, Row, Col, Card } from 'nhsuk-react-components'
import { Link } from 'react-router-dom';
import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';

export const ConfigHomePage = () => {
    return (
        <>
            <section className="nhsuk-hero">
                <Container>
                    <Row className="nhsuk-grid-row">
                        <Col width="two-thirds">
                            <div className="nhsuk-hero__wrapper app-hero__wrapper">
                                <h1 className="nhsuk-u-margin-bottom-4">Configuration</h1>
                                <p className="nhsuk-body-l nhsuk-u-margin-bottom-1">Area to manage configuration items.</p>
                            </div>
                        </Col>
                    </Row>
                </Container>
            </section>

            <section>
                <Container className="NELTopPadding">
                    <Card.Group>
                        <Card.GroupItem width="one-half">
                            <Card clickable>
                                <Card.Content>
                                    <Card.Heading className="nhsuk-card__heading nhsuk-heading-m">
                                        <Link to={'/configuration/suppliers'}>
                                            Suppliers
                                        </Link>
                                    </Card.Heading>
                                    <Card.Description>
                                        View, add, edit and remove Suppliers.
                                    </Card.Description>
                                </Card.Content>
                            </Card>
                        </Card.GroupItem>

                        <Card.GroupItem width="one-half">
                            <Card clickable>
                                <Card.Content>
                                    <Card.Heading className="nhsuk-card__heading nhsuk-heading-m">
                                        <Link to={'/configuration/dataTypes'}>
                                            Data Types
                                        </Link>
                                    </Card.Heading>
                                    <Card.Description>
                                        View, add, edit and remove Data Types.
                                    </Card.Description>
                                </Card.Content>
                            </Card>
                        </Card.GroupItem>

                        <Card.GroupItem width="one-half">
                            <Card clickable>
                                <Card.Content>
                                    <Card.Heading className="nhsuk-card__heading nhsuk-heading-m">
                                        <Link to={'/configuration/suppliers'}>
                                            Data Sets
                                        </Link>
                                    </Card.Heading>
                                    <Card.Description>
                                        View, add, edit and remove Data Sets.
                                    </Card.Description>
                                </Card.Content>
                            </Card>
                        </Card.GroupItem>
                    </Card.Group>
                </Container>
            </section>
        </>
    );
};
