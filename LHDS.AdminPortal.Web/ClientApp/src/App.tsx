import React from 'react';
import { MsalProvider } from '@azure/msal-react';
import { ReactQueryDevtools } from 'react-query/devtools'
import 'bootstrap/dist/css/bootstrap.min.css';
import { PageLayout } from './components/PageLayout';
import { Route, Routes } from 'react-router-dom';
import { Home } from './pages/home';
import { QueryClientProvider } from 'react-query';
import { queryClientGlobalOptions } from './brokers/apiBroker.globals';
import { IngestionTrackingHomepage } from './pages/IngestionTrackingHomepage';
import { IngestionTrackingPage } from './pages/ingestionTrackingPage';
import { ConfigHomePage } from './pages/configuration/configHomePage';
import { SuppliersPage } from './pages/configuration/suppliersPage';
import { OptOutSearch } from './pages/OptOutSearch';
import { OptOutUpload } from './pages/OptOutUpload';
import { PdsSearch } from './pages/PdsSearch';
import { PdsUpload } from './pages/PdsUpload';
import { SecuredRoute } from './components/SecuredRoute';
import securityPoints from './SecurityMatrix';
import { DataTypesPage } from './pages/configuration/dataTypesPage';
import { DataSetsPage } from './pages/configuration/dataSetsPage';

const App = ({ msalInstance }: any) => {
    return (
        <MsalProvider instance={msalInstance}>
            <QueryClientProvider client={queryClientGlobalOptions}>
                <PageLayout>
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/ingestionTracking" element={<SecuredRoute allowedRoles={securityPoints.ingestionTracking.view}><IngestionTrackingHomepage /></SecuredRoute>} />
                        <Route path="/ingestionTrackingDetail" element={<SecuredRoute allowedRoles={securityPoints.ingestionTracking.view}><IngestionTrackingPage /></SecuredRoute>} />
                        <Route path="/ingestionTrackingDetail/:ingestionTrackingId" element={<SecuredRoute allowedRoles={securityPoints.ingestionTracking.view}><IngestionTrackingPage /></SecuredRoute>} />
                        <Route path="/optOutSearch" element={<SecuredRoute allowedRoles={securityPoints.optOut.view}><OptOutSearch /></SecuredRoute>} />
                        <Route path="/optOutUpload" element={<SecuredRoute allowedRoles={securityPoints.optOut.view}><OptOutUpload /></SecuredRoute>} />
                        <Route path="/configuration" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><ConfigHomePage/></SecuredRoute>} />
                        <Route path="/configuration/suppliers" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><SuppliersPage /></SecuredRoute >} />
                        <Route path="/configuration/dataTypes" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataTypesPage /></SecuredRoute >} />
                        <Route path="/configuration/dataSets" element={<SecuredRoute allowedRoles={securityPoints.configuration.view}><DataSetsPage /></SecuredRoute >} />
                        <Route path="/pds" element={<SecuredRoute allowedRoles={securityPoints.pds.view}>< PdsSearch /></SecuredRoute >} />
                        <Route path="/pdsUpload" element={<SecuredRoute allowedRoles={securityPoints.pds.view}>< PdsUpload /></SecuredRoute >} />
                    </Routes>
                </PageLayout>
                <ReactQueryDevtools initialIsOpen={false} />
            </QueryClientProvider>
        </MsalProvider>
    );
}

export default App;