import 'bootstrap/dist/css/bootstrap.css';

import React from 'react';
import { createRoot } from 'react-dom/client';

import { BrowserRouter } from 'react-router-dom';
import App from './App';
import * as serviceWorkerRegistration from './serviceWorkerRegistration';
import reportWebVitals from './reportWebVitals';
import { msalConfig } from './authConfig';
import { PublicClientApplication } from "@azure/msal-browser";
import "react-toastify/dist/ReactToastify.css";
import ToastBroker from './brokers/toastBroker';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') || "\\";
const rootElement = document.getElementById('root');
const msalInstance = new PublicClientApplication(msalConfig);
const root = createRoot(rootElement!);

root.render(
    <BrowserRouter basename={baseUrl}>
        <App msalInstance={msalInstance} />
        <ToastBroker.Container />
    </BrowserRouter>);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();