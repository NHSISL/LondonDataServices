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

const App = ({ msalInstance }: any) => {
    return (
        <MsalProvider instance={msalInstance}>
            <QueryClientProvider client={queryClientGlobalOptions}>
                <PageLayout>
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/ingestionTracking" element={<IngestionTrackingHomepage />} />
                        <Route path="/ingestionTrackingDetail" element={<IngestionTrackingPage />} />
                        <Route path="/ingestionTrackingDetail/:ingestionTrackingId" element={<IngestionTrackingPage />} />
                        <Route path="/optOutSearch" element={<OptOutSearch />} />
                        <Route path="/optOutUpload" element={<OptOutUpload />} />
                        <Route path="/configuration" element={<ConfigHomePage/>} />
                        <Route path="/configuration/suppliers" element={<SuppliersPage />} />
                        <Route path="/pds" element={<PdsSearch />} />
                        <Route path="/pdsUpload" element={<PdsUpload />} />
                    </Routes>
                </PageLayout>
                <ReactQueryDevtools initialIsOpen={false} />
            </QueryClientProvider>
        </MsalProvider>
    );
}

export default App;