import React from 'react';
import { PublicLink, SecuredComponents } from '../components/links';
import { useIsAuthenticated } from '@azure/msal-react';
import { FeatureSwitch } from '../components/accessControl/featureSwitch';
import { FeatureDefinitions } from '../featureDefinitions';
import securityPoints from '../securityMatrix';
import CardBase from '../components/bases/components/Card/CardBase';
import CardBaseContent from '../components/bases/components/Card/CardBase.Content';
import CardBaseBody from '../components/bases/components/Card/CardBase.Body';
import { HomeUnAuthorised } from './homeUnAuth';
import { PageLayout } from '../components/pageLayout';

export const Home = () => {
    const isAuthenticated = useIsAuthenticated();
    return (
        <>
            <section>
                <div>
                    {isAuthenticated ? (
                        <PageLayout>
                            <div className="container-fluid">
                                <div className="container-fluid py-5 bg-primary text-white">
                                    
                                    <h1 className="display-5 fw-bold">
                                        {/*<img src="LHDLogoRound.png" alt="London Data Service logo" height="100" width="100" style={{marginRight: "20px"}} />*/}
                                        London Data Service
                                    </h1>
                                    <p className="col-md-8 fs-4">"Empowering Healthcare with London's GP Data Excellence"</p>
                                    <p></p>
                                </div>
                                <FeatureSwitch feature={FeatureDefinitions.IngestionTracking}>
                                    <div className="row">

                                        <div className="col-lg-4 col-md-6 col-sm-12">
                                            <SecuredComponents allowedRoles={securityPoints.ingestionTracking.view}>
                                                <CardBase>
                                                    <CardBaseBody>
                                                        <CardBaseContent>
                                                            <div className="me-3 mt-3">
                                                                <h5><PublicLink icon="" to="/ingestionTracking">View Ingestion Data</PublicLink></h5>
                                                                <div className="text-muted small">
                                                                    To search for the ingested data encrypted / decrypted.
                                                                </div>
                                                            </div>
                                                        </CardBaseContent>
                                                    </CardBaseBody>
                                                </CardBase>
                                            </SecuredComponents>
                                        </div>

                                        <div className="col-lg-4 col-md-6 col-sm-12">
                                            <SecuredComponents allowedRoles={securityPoints.optOut.view}>
                                                <CardBase>
                                                    <CardBaseBody>
                                                        <CardBaseContent>
                                                            <div className="me-3 mt-3">
                                                                <h5><PublicLink icon="" to="/optOutSearch">Search OptOut</PublicLink></h5>
                                                                <div className="text-muted small">
                                                                    To search for the latest Opt-Out status of a patient.
                                                                </div>
                                                            </div>
                                                        </CardBaseContent>
                                                    </CardBaseBody>
                                                </CardBase>
                                            </SecuredComponents>
                                        </div>

                                        <div className="col-lg-4 col-md-6 col-sm-12">
                                            <SecuredComponents allowedRoles={securityPoints.pds.view}>
                                                <CardBase>
                                                    <CardBaseBody>
                                                        <CardBaseContent>
                                                            <div className="me-3 mt-3">
                                                                <h5><PublicLink icon="" to="/pds">Search PDS</PublicLink></h5>
                                                                <div className="text-muted small">
                                                                    To search for patient demographic returned values from NHS Mesh.
                                                                </div>
                                                            </div>
                                                        </CardBaseContent>
                                                    </CardBaseBody>
                                                </CardBase>
                                            </SecuredComponents>
                                        </div>
                                    </div>
                                </FeatureSwitch>


                                <FeatureSwitch feature={FeatureDefinitions.TerminologyArtifact}>
                                    <div className="row">

                                        <div className="col-lg-4 col-md-6 col-sm-12">
                                            <SecuredComponents allowedRoles={securityPoints.terminologyArtifact.view}>
                                                <CardBase>
                                                    <CardBaseBody>
                                                        <CardBaseContent>
                                                            <div className="me-3 mt-3">
                                                                <h5><PublicLink icon="" to="/terminologyArtifact">View Terminology Data</PublicLink></h5>
                                                                <div className="text-muted small">
                                                                    To search for the terminology data.
                                                                </div>
                                                            </div>
                                                        </CardBaseContent>
                                                    </CardBaseBody>
                                                </CardBase>
                                            </SecuredComponents>
                                        </div>

                                        <div className="col-lg-4 col-md-6 col-sm-12">
                                           
                                        </div>

                                        <div className="col-lg-4 col-md-6 col-sm-12">
                                            
                                        </div>
                                    </div>
                                </FeatureSwitch>



                            </div>
                        </PageLayout>
                    ) : (
                            <div className="container-fluid text-dark bg-primary">
                            <HomeUnAuthorised></HomeUnAuthorised>
                        </div>
                    )}
                </div>
            </section>
        </>
    );
};
