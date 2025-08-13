import React from 'react';
import { MsalProvider } from '@azure/msal-react';
import { ReactQueryDevtools } from 'react-query/devtools'
import 'bootstrap/dist/css/bootstrap.min.css';
import { Route, Routes } from 'react-router-dom';
import { Home } from './pages/home';
import { QueryClientProvider } from 'react-query';
import { queryClientGlobalOptions } from './brokers/apiBroker.globals';
import { IngestionTrackingHomepage } from './pages/IngestionTrackingHomepage';
import { IngestionTrackingPage } from './pages/ingestionTrackingPage';
import { ConfigHomePage } from './pages/configuration/configHomePage';
import { SuppliersPage } from './pages/configuration/suppliersPage';
import { OptOutSearch } from './pages/optOutSearch';
import { OptOutUpload } from './pages/optOutUpload';
import { PdsSearch } from './pages/pdsSearch';
import { PdsUpload } from './pages/pdsUpload';
import { SecuredRoute } from './components/securedRoute';
import securityPoints from './securityMatrix';
import { DataTypesPage } from './pages/configuration/dataTypesPage';
import { DataSetsListPage } from './pages/configuration/dataSetsListPage';
import { DataSetPage } from './pages/configuration/dataSetPage';
import { DataSetSpecificationPage } from './pages/configuration/dataSetSpecificationPage';
import { SpecificationObjectPage } from './pages/configuration/specificationObjectPage';
import { ObjectColumnPage } from './pages/configuration/objectColumnPage';
import { TerminologyArtifactPage } from './pages/terminologyArtifactPage';
import { TerminologyArtifactDetailPage } from './pages/terminologyArtifactDetailPage';
import { SubscriberAgreementPage } from './pages/subscriberAgreementPage';
import { SubscriberAgreementDetailPage } from './pages/subscriberAgreementDetailPage';
import { SubscriberAgreementAddPage } from './pages/subscriberAgreementAddPage';
import { AddressPage } from './pages/addressPage';
import { AddressDetailPage } from './pages/addressDetailPage';
import { ResolvedAddressPage } from './pages/resolvedAddressPage';
import { ResolvedAddressDetailPage } from './pages/resolvedAddressDetailPage';

const App = ({ msalInstance }: any) => {
    return (
        <MsalProvider instance={msalInstance}>
            <QueryClientProvider client={queryClientGlobalOptions}>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/address" element={<SecuredRoute allowedRoles={securityPoints.address.view}><AddressPage /></SecuredRoute>} />
                    <Route path="/addressDetail/:addressId" element={<SecuredRoute allowedRoles={securityPoints.address.view}><AddressDetailPage /></SecuredRoute>} />
                    <Route path="/ingestionTracking" element={<SecuredRoute allowedRoles={securityPoints.ingestionTracking.view}><IngestionTrackingHomepage /></SecuredRoute>} />
                    <Route path="/ingestionTrackingDetail" element={<SecuredRoute allowedRoles={securityPoints.ingestionTracking.view}><IngestionTrackingPage /></SecuredRoute>} />
                    <Route path="/ingestionTrackingDetail/:ingestionTrackingId" element={<SecuredRoute allowedRoles={securityPoints.ingestionTracking.view}><IngestionTrackingPage /></SecuredRoute>} />
                    <Route path="/optOutSearch" element={<SecuredRoute allowedRoles={securityPoints.optOut.view}><OptOutSearch /></SecuredRoute>} />
                    <Route path="/optOutUpload" element={<SecuredRoute allowedRoles={securityPoints.optOut.view}><OptOutUpload /></SecuredRoute>} />
                    <Route path="/configuration" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><ConfigHomePage /></SecuredRoute>} />
                    <Route path="/configuration/suppliers" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><SuppliersPage /></SecuredRoute >} />
                    <Route path="/configuration/dataTypes" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataTypesPage /></SecuredRoute >} />
                    <Route path="/configuration/dataSets" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetsListPage /></SecuredRoute >} />
                    <Route path="/configuration/dataSet" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetPage /></SecuredRoute >} />
                    <Route path="/configuration/dataSet/:dataSetId" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetPage /></SecuredRoute>} />
                    <Route path="/configuration/dataSetSpecification" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetSpecificationPage /></SecuredRoute >} />
                    <Route path="/configuration/dataSetSpecification/:dataSetId" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetSpecificationPage /></SecuredRoute>} />
                    <Route path="/configuration/dataSetSpecification/:dataSetId/:dataSetSpecificationId" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetSpecificationPage /></SecuredRoute>} />
                    <Route path="/configuration/specificationObjectAdd/:dataSetSpecificationId/:dataSetId" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><SpecificationObjectPage /></SecuredRoute>} />
                    <Route path="/configuration/specificationObject/:specificationObjectId/:dataSetSpecificationId/" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><SpecificationObjectPage /></SecuredRoute>} />
                    <Route path="/configuration/objectColumn/:dataSetSpecificationId/:specificationObjectId" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><ObjectColumnPage /></SecuredRoute>} />
                    <Route path="/configuration/objectColumn/:specificationObjectId" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><ObjectColumnPage /></SecuredRoute>} />
                    <Route path="/configuration/objectColumn/:dataSetSpecificationId/:objectColumnId/:specificationObjectId/" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><ObjectColumnPage /></SecuredRoute>} />
                    <Route path="/pds" element={<SecuredRoute allowedRoles={securityPoints.pds.view}>< PdsSearch /></SecuredRoute >} />
                    <Route path="/pdsUpload" element={<SecuredRoute allowedRoles={securityPoints.pds.view}>< PdsUpload /></SecuredRoute >} />
                    <Route path="/terminologyArtifact" element={<SecuredRoute allowedRoles={securityPoints.terminologyArtifact.view}><TerminologyArtifactPage /></SecuredRoute>} />
                    <Route path="/terminologyArtifactDetail/:terminologyArtifactId" element={<SecuredRoute allowedRoles={securityPoints.terminologyArtifact.view}><TerminologyArtifactDetailPage /></SecuredRoute>} />
                    <Route path="/resolvedAddress" element={<SecuredRoute allowedRoles={securityPoints.resolvedAddress.view}><ResolvedAddressPage /></SecuredRoute>} />
                    <Route path="/resolvedAddressDetail/:resolvedAddressId" element={<SecuredRoute allowedRoles={securityPoints.resolvedAddress.view}><ResolvedAddressDetailPage /></SecuredRoute>} />
                    <Route path="/subscriberAgreements" element={<SecuredRoute allowedRoles={securityPoints.subscriberAgreement.view}><SubscriberAgreementPage /></SecuredRoute>} />
                    <Route path="/subscriberAgreement" element={<SecuredRoute allowedRoles={securityPoints.subscriberAgreement.add}><SubscriberAgreementAddPage /></SecuredRoute>} />
                    <Route path="/subscriberAgreement/new" element={<SecuredRoute allowedRoles={securityPoints.subscriberAgreement.add}><SubscriberAgreementAddPage /></SecuredRoute>} />
                    <Route path="/subscriberAgreementDetail/:subscriberAgreementId" element={<SecuredRoute allowedRoles={securityPoints.subscriberAgreement.view}><SubscriberAgreementDetailPage /></SecuredRoute>} />
                </Routes>
                <ReactQueryDevtools initialIsOpen={false} />
            </QueryClientProvider>
        </MsalProvider>
    );
}

export default App;