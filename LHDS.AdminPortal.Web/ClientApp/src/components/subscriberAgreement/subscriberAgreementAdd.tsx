import React, { FunctionComponent } from "react";
import { Button, Form, FormGroup } from "react-bootstrap";
import { Label } from "nhsuk-react-components";
import ButtonBase from "../bases/buttons/ButtonBase";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import TextAreaInputBase from "../bases/inputs/TextAreaInputBase";
import TextInputBase from "../bases/inputs/TextInputBase";
import { set } from "lodash";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faCopy, faTicket } from "@fortawesome/free-solid-svg-icons";
import { useNavigate } from "react-router-dom";

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
        navigate('/subscriberAgreementDetail/TODO'); 
    }

    const copySSHKeyToClipboard = (event:any) => {
        event.preventDefault();          
        navigator.clipboard.writeText(sshPublicKey);
        setSshKeyCopied(true);
    }

    const copyGPGKeyToClipboard = (event:any) => {
        event.preventDefault();  
        navigator.clipboard.writeText(gpgPublicKey);
        setGpgKeyCopied(true);
    }

    return (
        <CardBase>
            <CardBaseBody>
                <CardBaseTitle>
                    Subscriber Agreements
                </CardBaseTitle>
                <CardBaseContent>
                    <Form>
                        <TextInputBase  id="Subscriber Agreement Short Name" name="Subscriber Agreement Short Name" label="Subscriber Agreement Short Name"  onChange={(e) => { setSubscriberAgreementShortName(e.target.value)}} value={subscriberAgreementShortName} />
                        <ButtonBase onClick={addSubscriberAgreement}>Create Subscriber Agreement and Generate Keys</ButtonBase>
                    </Form>
                </CardBaseContent>
            </CardBaseBody>
        </CardBase>
    );
}

export default SubscriberAgreementAdd;