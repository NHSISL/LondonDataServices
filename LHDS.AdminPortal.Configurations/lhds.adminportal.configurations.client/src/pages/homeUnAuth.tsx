import CardBase from '../components/bases/components/Card/CardBase';
import CardBaseContent from '../components/bases/components/Card/CardBase.Content';
import CardBaseBody from '../components/bases/components/Card/CardBase.Body';
import CardBaseHeader from '../components/bases/components/Card/CardBase.Header';
import CardBaseTitle from '../components/bases/components/Card/CardBase.Title';

export const HomeUnAuthorised = () => {
    return (
        <div className="m-3">
            <div className="container center max-width-400 min-height-600 vh-100">
                
                <CardBase classes="border">
                    <div className="logo-container">
                        <img src="LHDLogoRound.png" alt="London Data Service logo" className="logo" height="50" width="50" />
                    </div>
                    <CardBaseBody classes="">
                        <CardBaseHeader>
                            <CardBaseTitle>Login</CardBaseTitle>
                        </CardBaseHeader>
                        <CardBaseContent>
                            <br />                  
                            <p className="welcome-text">
                                <strong className="lead text-hero">Welcome!</strong> You've reached the <span className="lead text-hero">London Data Service</span> Admin Portal.
                            </p>
                            <p>
                                To access all system features, please log in. For access requests, kindly contact your Manager.
                            </p>

                            {/*<ButtonBase*/}
                            {/*    className="btn btn-primary mt-3"*/}
                            {/*    onClick={() => instance.loginRedirect(loginRequest)}>*/}
                            {/*    Login*/}
                            {/*</ButtonBase>*/}
                        </CardBaseContent>
                    </CardBaseBody>
                </CardBase>

                <div className="copyright">&copy; 2023 North East London ICB</div>
            </div>
        </div>
    );
};
