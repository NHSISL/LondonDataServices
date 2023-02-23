import { expect, it } from '@jest/globals'
import React from 'react';
import { createRoot } from 'react-dom/client';
import { MemoryRouter } from 'react-router-dom';
import App from './App';
import { MsalReactTester } from 'msal-react-tester';
import { act } from 'react-dom/test-utils'

describe('App Text', () => {
    let msalTester;
    let container;

    beforeEach(() => {
        // new instance of msal tester for each test:
        container = document.createElement('div');
        msalTester = new MsalReactTester();
        msalTester.spyMsal();
        // or new MsalReactTester("Redirect") / new MsalReactTester("Popup")

        // Ask msal-react-tester to handle and mock all msal-react processes:
    });

    afterEach(() => {
        // reset msal-react-tester
        msalTester.resetSpyMsal();
    });

    it('renders without crashing for a not logged in user', async () => {
        msalTester.isNotLogged();

        await act(async () => {
            const root = createRoot(container);
            root.render(
                <MemoryRouter>
                    <App msalInstance={msalTester.client} />
                </MemoryRouter>);
            await new Promise(resolve => setTimeout(resolve));
        });

        const header = container.querySelector('h1');
        expect(header.textContent).toBe('GP Premises Manager');
    });


    it('renders without crashing for a logged in user', async () => {
        msalTester.isLogged();

        await act(async () => {
            const root = createRoot(container);
            root.render(
                <MemoryRouter>
                    <App msalInstance={msalTester.client} />
                </MemoryRouter>);
            await new Promise(resolve => setTimeout(resolve));
        });

        const header = container.querySelector('h1');
        expect(header.textContent).toBe('GP Premises Manager');
    });

})