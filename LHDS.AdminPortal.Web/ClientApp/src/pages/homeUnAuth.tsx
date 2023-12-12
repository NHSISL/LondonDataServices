import React from 'react';
import { useMsal } from '@azure/msal-react';
import { loginRequest } from '../authConfig';
import CardBase from '../components/bases/components/Card/CardBase';
import CardBaseContent from '../components/bases/components/Card/CardBase.Content';
import CardBaseBody from '../components/bases/components/Card/CardBase.Body';
import CardBaseHeader from '../components/bases/components/Card/CardBase.Header';
import CardBaseTitle from '../components/bases/components/Card/CardBase.Title';
import ButtonBase from '../components/bases/buttons/ButtonBase';

export const HomeUnAuthorised = () => {
    const { instance } = useMsal();

    return (
        <>
            <div className="container center max-width-400 min-height-600">
                <CardBase classes="border ">
                    <CardBaseBody classes="">
                        <CardBaseHeader>
                            <CardBaseTitle>Login</CardBaseTitle>
                        </CardBaseHeader>
                        <CardBaseContent >
                                <br/>                     
                            <p>Welcome to the London Data Service Admin Portal.</p>

                            <p>To unlock all the features of this system login,
                                For access requests, please contact your Manager.
                            </p>

                            <ButtonBase
                                className="btn btn-primary mt-3"
                                onClick={() => instance.loginRedirect(loginRequest)}>
                                Login
                            </ButtonBase>
                        </CardBaseContent>
                    </CardBaseBody>
                </CardBase>
            </div>
        </>
    );
};
