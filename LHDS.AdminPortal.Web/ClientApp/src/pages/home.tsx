import React from 'react';
import { PublicLink, SecuredComponents } from '../components/Links';
import { useIsAuthenticated, useMsal } from '@azure/msal-react';
import { loginRequest } from '../authConfig';
import { Button, Card, Container } from 'react-bootstrap';
import { FeatureSwitch } from '../components/accessControl/FeatureSwitch';
import { FeatureDefinitions } from '../featureDefinitions';
import securityPoints from '../SecurityMatrix';
import CardBase from '../components/bases/components/Card/CardBase';
import CardBaseContent from '../components/bases/components/Card/CardBase.Content';
import CardBaseHeader from '../components/bases/components/Card/CardBase.Header';
import CardBaseBody from '../components/bases/components/Card/CardBase.Body';
import CardBaseTitle from '../components/bases/components/Card/CardBase.Title';
import ButtonBase from '../components/bases/buttons/ButtonBase';

export const Home = () => {
    const isAuthenticated = useIsAuthenticated();
    const { instance } = useMsal();
    return (
        <>

            <div className="container-fluid py-5 bg-primary text-white">
                <h1 className="display-5 fw-bold">London Data Service</h1>
                <p className="col-md-8 fs-4">"Empowering Healthcare with London's GP Data Excellence!"</p>
            </div>

            <section>
                <div>
                    {isAuthenticated ? (
                        <div className="container-fluid">

                            <FeatureSwitch feature={FeatureDefinitions.IngestionTracking}>
                                <div className="row">

                                    <div className="col-sm-4">
                                        <SecuredComponents allowedRoles={securityPoints.ingestionTracking.view}>
                                            <CardBase>
                                                <CardBaseBody>
                                                    <CardBaseTitle>
                                                        <PublicLink to="/ingestionTracking">Ingestion Tracking</PublicLink>
                                                    </CardBaseTitle>
                                                    <CardBaseContent>
                                                        View Ingestion Data.
                                                    </CardBaseContent>
                                                </CardBaseBody>
                                            </CardBase>
                                        </SecuredComponents>
                                    </div>

                                    <div className="col-sm-4">
                                        <SecuredComponents allowedRoles={securityPoints.optOut.view}>
                                            <CardBase>
                                                <CardBaseBody>
                                                    <CardBaseTitle>
                                                        <PublicLink to="/optOutSearch">Search OptOut</PublicLink>
                                                    </CardBaseTitle>
                                                    <CardBaseContent>
                                                        Search Opt Out.
                                                    </CardBaseContent>
                                                </CardBaseBody>
                                            </CardBase>
                                        </SecuredComponents>
                                    </div>

                                    <div className="col-sm-4">
                                        <SecuredComponents allowedRoles={securityPoints.pds.view}>
                                            <CardBase>
                                                <CardBaseBody>
                                                    <CardBaseTitle>
                                                        <PublicLink to="/pds">Search PDS</PublicLink>
                                                    </CardBaseTitle>
                                                    <CardBaseContent>
                                                        Search Pds.
                                                    </CardBaseContent>
                                                </CardBaseBody>
                                            </CardBase>
                                        </SecuredComponents>
                                    </div>
                                </div>
                            </FeatureSwitch>
                        </div>
                    ) : (
                        <div className="container mt-5">
                            <CardBase>
                                <CardBaseBody>
                                    <CardBaseTitle>
                                        <a href="#" onClick={() => instance.loginRedirect(loginRequest)}>
                                            Login to continue.
                                        </a>
                                    </CardBaseTitle>
                                    <CardBaseContent>
                                        To unlock all the features of this system,
                                        please

                                        <ButtonBase
                                            className="btn btn-primary ms-2 me-2"
                                            onClick={() => instance.loginRedirect(loginRequest)}>
                                            Login
                                        </ButtonBase>.

                                        For access requests, please contact your Manager.
                                    </CardBaseContent>
                                </CardBaseBody>
                            </CardBase>
                        </div>
                    )}
                </div>
            </section >

        </>
    );
};
