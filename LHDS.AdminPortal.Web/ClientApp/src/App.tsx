import React from 'react';
import './custom.css'
import { MsalProvider } from '@azure/msal-react';
import { ReactQueryDevtools } from 'react-query/devtools'
import "./styles/App.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import { PageLayout } from './components/PageLayout';
import { Route, Routes } from 'react-router-dom';
import { Home } from './pages/home';
import { QueryClientProvider } from 'react-query';
import { queryClientGlobalOptions } from './brokers/apiBroker.globals';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { AppInsightsContext, ReactPlugin } from '@microsoft/applicationinsights-react-js';

var reactPlugin = new ReactPlugin();
var appInsights = new ApplicationInsights({
    config: {
       
    }
});
appInsights.loadAppInsights();

const App = ({ msalInstance }: any) => {
    return (
        <AppInsightsContext.Provider value={reactPlugin} >
            <MsalProvider instance={msalInstance}>
                <QueryClientProvider client={queryClientGlobalOptions}>
                    <PageLayout>
                        <Routes>
                            <Route path="/" element={<Home />} />
                        </Routes>
                    </PageLayout>
                    <ReactQueryDevtools initialIsOpen={false} />
                </QueryClientProvider>
            </MsalProvider>
        </AppInsightsContext.Provider>
    );
}
export default App;