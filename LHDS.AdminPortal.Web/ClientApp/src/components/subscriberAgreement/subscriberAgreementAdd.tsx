import React, { FunctionComponent } from "react";
import { Form } from "react-bootstrap";
import ButtonBase from "../bases/buttons/ButtonBase";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import TextInputBase from "../bases/inputs/TextInputBase";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faKey } from "@fortawesome/free-solid-svg-icons";

interface SubscriberAgreementAddProps {
    children?: React.ReactNode;
}

const SubscriberAgreementAdd: FunctionComponent<SubscriberAgreementAddProps> = (props) => {
    const {
        children
    } = props;

    const [added, setAdded] = React.useState<boolean>(false);
    const [subscriberAgreementShortName, setSubscriberAgreementShortName] = React.useState<string>("");
    const [sshPublicKey, setSshPublicKey] = React.useState<string>("");
    const [gpgPublicKey, setGpgPublicKey] = React.useState<string>("");
    const [sshKeyCopied, setSshKeyCopied] = React.useState<boolean>(false);
    const [gpgKeyCopied, setGpgKeyCopied] = React.useState<boolean>(false);
    const navigate = useNavigate();

    const addSubscriberAgreement = () => { 
        setSshPublicKey("ssh public key");
        setGpgPublicKey("gpg public key");
        setAdded(true)
        navigate('/subscriberAgreement'); 
    }

    return (
        <CardBase>
            <CardBaseBody>
                <CardBaseTitle>
                    Subscriber Agreements
                </CardBaseTitle>
                <CardBaseContent>
                    <Form>

                        <TextInputBase
                            id="Subscriber Agreement Short Name"
                            name="Subscriber Agreement Short Name"
                            label="Subscriber Agreement Short Name"
                            onChange={(e) => { setSubscriberAgreementShortName(e.target.value) }}
                            value={subscriberAgreementShortName} />

                        <br />
                        <ButtonBase onClick={addSubscriberAgreement} info>
                            Create Subscriber Agreement and Generate Keys &nbsp;
                            <FontAwesomeIcon icon={faKey} title="required" />
                        </ButtonBase>

                        <br /><br />
                        <p>INFO: Description of what will happen</p>
                    </Form>
                </CardBaseContent>
            </CardBaseBody>
        </CardBase>
    );
}

export default SubscriberAgreementAdd;