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
import { SecuredRoute } from './components/SecuredRoute';
import securityPoints from './SecurityMatrix';
import { ConfigHomePage } from './pages/configuration/configHomePage';
import { SuppliersPage } from './pages/configuration/suppliersPage';

const App = ({ msalInstance }: any) => {
    return (
        <MsalProvider instance={msalInstance}>
            <QueryClientProvider client={queryClientGlobalOptions}>
                <PageLayout>
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/ingestionTracking" element={<IngestionTrackingHomepage />} />
                        <Route path="/ingestionTrackingDetail" element={<IngestionTrackingPage />} />

                        <Route path="/configuration" element={<SecuredRoute allowedRoles={securityPoints.configNavigation.view}><ConfigHomePage /></SecuredRoute>} />
                        <Route path="/configuration/suppliers" element={<SecuredRoute allowedRoles={securityPoints.configNavigation.view}><SuppliersPage /></SecuredRoute>} />
                    </Routes>
                </PageLayout>
                <ReactQueryDevtools initialIsOpen={false} />
            </QueryClientProvider>
        </MsalProvider>
    );
}
export default App;