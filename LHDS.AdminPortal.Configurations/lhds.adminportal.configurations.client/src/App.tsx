/* eslint-disable @typescript-eslint/no-explicit-any */
import { createBrowserRouter, Navigate, RouterProvider } from 'react-router-dom';
import './App.css';
import Root from './components/root';
import ErrorPage from './errors/error';
import { MsalProvider } from '@azure/msal-react';
import { QueryClientProvider } from '@tanstack/react-query';
import { queryClientGlobalOptions } from './brokers/apiBroker.globals';
import { Home } from './pages/home';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import "react-toastify/dist/ReactToastify.css";
import ToastBroker from './brokers/toastBroker';
import { IngestionTrackingHomepage } from './pages/IngestionTrackingHomepage';
import { IngestionTrackingPage } from './pages/ingestionTrackingPage';
import { OptOutSearchPage } from './pages/optOutSearchPage';
import { OptOutUploadPage } from './pages/optOutUploadPage';
import { PdsSearchPage } from './pages/pdsSearchPage';
import { PdsUploadPage } from './pages/pdsUploadPage';
import { TerminologyArtifactDetailPage } from './pages/terminologyArtifactDetailPage';
import { TerminologyArtifactPage } from './pages/terminologyArtifactPage';
import { ConfigHomePage } from './pages/configuration/configHomePage';
import { SuppliersPage } from './pages/configuration/suppliersPage';
import { SubscriberAgreementPage } from './pages/subscriberAgreementPage';
import { SubscriberAgreementAddPage } from './pages/subscriberAgreementAddPage';
import { SubscriberAgreementDetailPage } from './pages/subscriberAgreementDetailPage';
import { DataTypesPage } from './pages/configuration/dataTypesPage';
import { DataSetsListPage } from './pages/configuration/dataSetsListPage';
import { DataSetPage } from './pages/configuration/dataSetPage';
import { DataSetSpecificationPage } from './pages/configuration/dataSetSpecificationPage';
import { SpecificationObjectPage } from './pages/configuration/specificationObjectPage';
import { ObjectColumnPage } from './pages/configuration/objectColumnPage';
import { AddressPage } from './pages/addressPage';
import { AddressDetailPage } from './pages/addressDetailPage';
import { ResolvedAddressPage } from './pages/resolvedAddressPage';
import { ResolvedAddressDetailPage } from './pages/resolvedAddressDetailPage';
import { SecuredRoute } from './components/securitys/securedRoutes';
import securityPoints from './securityMatrix';


function App({ instance }: any) {

    const router = createBrowserRouter([
        {
            path: "/",
            element: <Root />,
            errorElement: <ErrorPage />,
            children: [
                {
                    path: "home",
                    element: <Home />
                },
                {
                    path: "/ingestionTracking",
                    element: (<SecuredRoute allowedRoles={securityPoints.ingestionTracking.view}><IngestionTrackingHomepage /></SecuredRoute>)
                },
                {
                    path: "/ingestionTracking",
                    element: (<SecuredRoute allowedRoles={securityPoints.ingestionTracking.view}><IngestionTrackingPage /></SecuredRoute>)
                },
                {
                    path: "/ingestionTrackingDetail/:ingestionTrackingId",
                    element: (<SecuredRoute allowedRoles={securityPoints.ingestionTracking.view}><IngestionTrackingPage /></SecuredRoute>)
                },
                {
                    path: "/optOutSearch",
                    element: (<SecuredRoute allowedRoles={securityPoints.optOut.view}><OptOutSearchPage /></SecuredRoute>)
                },
                {
                    path: "/optOutUpload",
                    element: (<SecuredRoute allowedRoles={securityPoints.optOut.view}><OptOutUploadPage /></SecuredRoute>)
                },
                {
                    path: "/pds",
                    element: (<SecuredRoute allowedRoles={securityPoints.pds.view}><PdsSearchPage /></SecuredRoute>)
                },
                {
                    path: "/pdsUpload",
                    element: (<SecuredRoute allowedRoles={securityPoints.pds.view}><PdsUploadPage /></SecuredRoute>)
                },
                {
                    path: "/terminologyArtifact",
                    element: (<SecuredRoute allowedRoles={securityPoints.terminologyArtifact.view}><TerminologyArtifactPage /></SecuredRoute>)
                },
                {
                    path: "/terminologyArtifactDetail/:terminologyArtifactId",
                    element: (<SecuredRoute allowedRoles={securityPoints.terminologyArtifact.view}><TerminologyArtifactDetailPage /></SecuredRoute>)
                },
                {
                    path: "/address",
                    element: (<SecuredRoute allowedRoles={securityPoints.address.view}><AddressPage /></SecuredRoute>)
                },
                {
                    path: "/addressDetail/:addressId",
                    element: (<SecuredRoute allowedRoles={securityPoints.address.view}><AddressDetailPage /></SecuredRoute>)
                },
                {
                    path: "/resolvedAddress",
                    element: (<SecuredRoute allowedRoles={securityPoints.resolvedAddress.view}><ResolvedAddressPage /></SecuredRoute>)
                },
                {
                    path: "/resolvedAddressDetail/:resolvedAddressId",
                    element: (<SecuredRoute allowedRoles={securityPoints.resolvedAddress.view}><ResolvedAddressDetailPage /></SecuredRoute>)
                },
                {
                    path: "/configuration",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><ConfigHomePage /></SecuredRoute>)
                },
                {
                    path: "/configuration/suppliers",
                    element: (<SecuredRoute allowedRoles={securityPoints.suppliers.view}><SuppliersPage /></SecuredRoute>)
                },
                {
                    path: "/subscriberAgreements",
                    element: (<SecuredRoute allowedRoles={securityPoints.subscriberAgreement.view}><SubscriberAgreementPage /></SecuredRoute>)
                },
                {
                    path: "/subscriberAgreement/new",
                    element: (<SecuredRoute allowedRoles={securityPoints.subscriberAgreement.view}><SubscriberAgreementAddPage /></SecuredRoute>)
                },
                {
                    path: "/subscriberAgreementDetail/:subscriberAgreementId",
                    element: (<SecuredRoute allowedRoles={securityPoints.subscriberAgreement.view}><SubscriberAgreementDetailPage /></SecuredRoute>)
                },
                {
                    path: "/configuration/dataTypes",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataTypesPage /></SecuredRoute>)
                },
                {
                    path: "/configuration/dataSets",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetsListPage /></SecuredRoute>)
                },
                {
                    path: "/configuration/dataSet",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetPage /></SecuredRoute>)
                },
                {
                    path: "/configuration/dataSet/:dataSetId",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetPage /></SecuredRoute>)
                },
                {
                    path: "/configuration/dataSetSpecification",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetSpecificationPage /></SecuredRoute>)
                },
                {
                    path: "/configuration/dataSetSpecification/:dataSetId",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetSpecificationPage /></SecuredRoute>)
                },
                {
                    path: "/configuration/dataSetSpecification/:dataSetId/:dataSetSpecificationId",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetSpecificationPage /></SecuredRoute>)
                },
                {
                    path: "/configuration/specificationObjectAdd/:dataSetSpecificationId/:dataSetId",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><SpecificationObjectPage /></SecuredRoute>)
                },
                {
                    path: "/configuration/specificationObject/:specificationObjectId/:dataSetSpecificationId/",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><SpecificationObjectPage /></SecuredRoute>)
                },
                {
                    path: "configuration/objectColumn/:dataSetSpecificationId/:specificationObjectId",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><ObjectColumnPage /></SecuredRoute>)
                },
                {
                    path: "configuration/objectColumn/:specificationObjectId",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><ObjectColumnPage /></SecuredRoute>)
                },
                {
                    path: "configuration/objectColumn/:dataSetSpecificationId/:objectColumnId/:specificationObjectId/",
                    element: (<SecuredRoute allowedRoles={securityPoints.configuration.view}><ObjectColumnPage /></SecuredRoute>)
                },
                {
                    index: true,
                    element:<Navigate to="/home" />
                }
            ]
        }
    ]);

    return (
        <>
            <MsalProvider instance={instance}>
                <QueryClientProvider client={queryClientGlobalOptions}>
                    <RouterProvider router={router} />
                    <ReactQueryDevtools initialIsOpen={false} />
                </QueryClientProvider>
                <ToastBroker.Container />
            </MsalProvider>
        </>
    );
}

export default App;