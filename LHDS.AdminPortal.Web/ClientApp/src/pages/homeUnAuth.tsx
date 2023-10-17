import React from 'react';
import { useMsal } from '@azure/msal-react';
import { loginRequest } from '../authConfig';
import CardBase from '../components/bases/components/Card/CardBase';
import CardBaseContent from '../components/bases/components/Card/CardBase.Content';
import CardBaseBody from '../components/bases/components/Card/CardBase.Body';
import ButtonBase from '../components/bases/buttons/ButtonBase';

export const HomeUnAuthorised = () => {
    const { instance } = useMsal();

    return (
        <>
            <div className="container center max-width-400 min-height-600">
                <CardBase>
                    <CardBaseBody>
                        <CardBaseContent>
                            <img src="/LHDLogo.png"
                                height="150"
                                width="150"
                                alt="logo"
                                className="mb-3 mt-3"
                                style={{
                                            display: 'block',  // Set the display property to block
                                            margin: '0 auto',   // Center horizontally with auto margins
                                    }} />
                           
                            <p>Welcome to the London Data Service Admin Portal.</p>

                            <p>To unlock all the features of this system login,
                                For access requests, please contact your Manager.
                            </p>

                            <ButtonBase
                                className="btn btn-primary"
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
