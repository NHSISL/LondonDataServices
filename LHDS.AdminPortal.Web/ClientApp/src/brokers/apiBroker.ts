import axios from 'axios';
import { InteractionRequiredAuthError, PublicClientApplication } from "@azure/msal-browser";
import { loginRequest, msalConfig } from '../authConfig';

class ApiBroker {

    baseUrl = process.env.REACT_APP_API;
    msalInstance = new PublicClientApplication(msalConfig);

    private async acquireAccessToken() {
        const activeAccount = this.msalInstance.getActiveAccount();
        const accounts = this.msalInstance.getAllAccounts();
        
        const request = {
            scopes: loginRequest.scopes,
            account: activeAccount || accounts[0]
        };

        await this.msalInstance.acquireTokenSilent(request).then(response => {
            return response.accessToken;
        }).catch(async (error) => {
            if (error instanceof InteractionRequiredAuthError) {
                // fallback to interaction when silent call fails
                return this.msalInstance.acquireTokenRedirect(request);
            }
        }).catch(error => {
            console.log(error);
        });

        const authResult = await this.msalInstance.acquireTokenSilent(request);
        
        return authResult.accessToken;
    }

    private async config() {
        const accessToken = await this.acquireAccessToken();
        if (accessToken) {
            return { headers: { 'Authorization': 'Bearer ' + await this.acquireAccessToken() } }
        }
        
        return {};
    }

    public async GetAsync(queryFragment: string) {
        const url = `${this.baseUrl}${queryFragment}`;
        return axios.get(url, await this.config());
    }

    public async GetAsyncAbsolute(absoluteUri: string) {
        return axios.get(absoluteUri, await this.config());
    }

    public async PostAsync(relativeUrl: string, data: any) {
        const url = `${this.baseUrl}${relativeUrl}`;

        return axios.post(url,
            data,
            await this.config()
        );
    }

    public async PostFormAsync(relativeUrl: string, data: FormData) {
        const url = `${this.baseUrl}${relativeUrl}`;

        const headers = {
            'Authorization': 'Bearer ' + await this.acquireAccessToken(),
            "content-type" : 'multipart/form-data'
        }

        return axios.post(url,
            data,
            {headers}
        );
    }

    public async PutAsync(relativeUrl: string, data: any) {
        const url = `${this.baseUrl}${relativeUrl}`;

        return axios.put(url, data, await this.config());
    }

    public async DeleteAsync(relativeUrl: string) {
        const url = `${this.baseUrl}${relativeUrl}`;

        return axios.delete(url, await this.config());
    }
}

export default ApiBroker;