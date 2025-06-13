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
                    element: (<SecuredRoute allowedRoles={securityPoints.address.view}><IngestionTrackingHomepage/></SecuredRoute>)
                },
                {
                    path: "/ingestionTracking",
                    element: <IngestionTrackingPage />
                },
                {
                    path: "/ingestionTrackingDetail/:ingestionTrackingId",
                    element: <IngestionTrackingPage />
                },
                {
                    path: "/optOutSearch",
                    element: <OptOutSearchPage />
                },
                {
                    path: "/optOutUpload",
                    element: <OptOutUploadPage />
                },
                {
                    path: "/pds",
                    element: <PdsSearchPage />
                },
                {
                    path: "/pdsUpload",
                    element: <PdsUploadPage />
                },
                {
                    path: "/terminologyArtifact",
                    element: <TerminologyArtifactPage />
                },
                {
                    path: "/terminologyArtifactDetail/:terminologyArtifactId",
                    element: <TerminologyArtifactDetailPage />
                },
                {
                    path: "/address",
                    element: <AddressPage />
                },
                {
                    path: "/addressDetail/:addressId",
                    element: <AddressDetailPage />
                },
                {
                    path: "/resolvedAddress",
                    element: <ResolvedAddressPage />
                },
                {
                    path: "/resolvedAddressDetail/:resolvedAddressId",
                    element: <ResolvedAddressDetailPage />
                },
                {
                    path: "/configuration",
                    element: <ConfigHomePage />
                },
                {
                    path: "/configuration/suppliers",
                    element: <SuppliersPage />
                },
                {
                    path: "/subscriberAgreements",
                    element: <SubscriberAgreementPage />
                },
                {
                    path: "/subscriberAgreement/new",
                    element: <SubscriberAgreementAddPage />
                },
                {
                    path: "/subscriberAgreementDetail/:subscriberAgreementId",
                    element: <SubscriberAgreementDetailPage />
                },
                {
                    path: "/configuration/dataTypes",
                    element: <DataTypesPage />
                },
                {
                    path: "/configuration/dataSets",
                    element: <DataSetsListPage />
                },
                {
                    path: "/configuration/dataSet",
                    element: <DataSetPage />
                },
                {
                    path: "/configuration/dataSet/:dataSetId",
                    element: <DataSetPage />
                },
                {
                    path: "/configuration/dataSetSpecification",
                    element: <DataSetSpecificationPage />
                },
                {
                    path: "/configuration/dataSetSpecification/:dataSetId",
                    element: <DataSetSpecificationPage />
                },
                {
                    path: "/configuration/dataSetSpecification/:dataSetId/:dataSetSpecificationId",
                    element: <DataSetSpecificationPage />
                },
                {
                    path: "/configuration/specificationObjectAdd/:dataSetSpecificationId/:dataSetId",
                    element: <SpecificationObjectPage />
                },
                {
                    path: "/configuration/specificationObject/:specificationObjectId/:dataSetSpecificationId/",
                    element: <SpecificationObjectPage />
                },
                {
                    path: "configuration/objectColumn/:dataSetSpecificationId/:specificationObjectId",
                    element: <ObjectColumnPage />
                },
                {
                    path: "configuration/objectColumn/:specificationObjectId",
                    element: <ObjectColumnPage />
                },
                {
                    path: "configuration/objectColumn/:dataSetSpecificationId/:objectColumnId/:specificationObjectId/",
                    element: <ObjectColumnPage />
                },
                {
                    index: true,
                    element: <Navigate to="/home" />
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