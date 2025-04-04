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
                    element: <IngestionTrackingHomepage />
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
                    path: "/configuration",
                    element: <ConfigHomePage />
                },
                {
                    path: "/configuration/suppliers",
                    element: <SuppliersPage />
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