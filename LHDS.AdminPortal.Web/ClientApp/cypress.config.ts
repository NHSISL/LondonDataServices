import { defineConfig } from "cypress";

const randomChar = () => {
    return String.fromCharCode((Math.random() * 25) + 97)
}

export default defineConfig({
    // All secrets are loaded from cypress.env.json (gitignored) or CI environment variables.
    // See cypress.env.json.example for the list of required keys.
    // Do NOT add credentials here — this file is committed to source control.

    e2e: {
        baseUrl: "https://localhost:44405/",
        hideXHRInCommandLog: true,
        setupNodeEvents(on, config) {
            // implement node event listeners here
        },
    },

    component: {
        devServer: {
            framework: "create-react-app",
            bundler: "webpack",
        },
    },

    testdata: {
        practiceName: Array(2).fill('').map(x => randomChar()).join('')
    },
});
