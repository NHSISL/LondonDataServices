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
import { OptOutSearch } from './pages/OptOutSearch';
import { OptOutUpload } from './pages/OptOutUpload';
import { PdsSearch } from './pages/PdsSearch';
import { PdsUpload } from './pages/PdsUpload';
import { SecuredRoute } from './components/SecuredRoute';
import securityPoints from './SecurityMatrix';
import { DataTypesPage } from './pages/configuration/dataTypesPage';
import { DataSetsListPage } from './pages/configuration/dataSetsListPage';
import { DataSetPage } from './pages/configuration/dataSetPage';
import { DataSetSpecificationPage } from './pages/configuration/dataSetSpecificationPage';
import { SpecificationObjectPage } from './pages/configuration/specificationObjectPage';
import { ObjectColumnPage } from './pages/configuration/ObjectColumPage';

const App = ({ msalInstance }: any) => {
    return (
        <MsalProvider instance={msalInstance}>
            <QueryClientProvider client={queryClientGlobalOptions}>
               
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
                    </Routes>
                
                <ReactQueryDevtools initialIsOpen={false} />
            </QueryClientProvider>
        </MsalProvider>
    );
}

export default App;