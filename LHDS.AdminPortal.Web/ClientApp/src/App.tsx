import React from 'react';
import { MsalProvider } from '@azure/msal-react';
import { ReactQueryDevtools } from 'react-query/devtools'
import 'bootstrap/dist/css/bootstrap.min.css';
import { PageLayout } from './components/PageLayout';
import { SuppliersHomepage } from './pages/suppliersHomepage';
import { Route, Routes } from 'react-router-dom';
import { Home } from './pages/home';
import { QueryClientProvider } from 'react-query';
import { queryClientGlobalOptions } from './brokers/apiBroker.globals';

const App = ({ msalInstance }: any) => {
    return (
            <MsalProvider instance={msalInstance}>
                <QueryClientProvider client={queryClientGlobalOptions}>
                    <PageLayout>
                        <Routes>
                            <Route path="/" element={<Home />} />
                        <Route path="/suppliers" element={<SuppliersHomepage />} />
                        </Routes>
                    </PageLayout>
                    <ReactQueryDevtools initialIsOpen={false} />
                </QueryClientProvider>
            </MsalProvider>
    );
}
export default App;