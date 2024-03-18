import React, { ChangeEvent, FunctionComponent, useState } from "react";
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
import { subscriberCredentialViewService } from "../../services/views/subscriberCredentials/subscriberCredentialViewService";
import { SubscriberCredentialView } from "../../models/views/components/subscriberCredentials/subscriberCredentialView";
import { Guid } from "guid-typescript";

interface SubscriberAgreementAddProps {
    children?: React.ReactNode;
}

const SubscriberAgreementAdd: FunctionComponent<SubscriberAgreementAddProps> = (props) => {
    const { children } = props;

    const navigate = useNavigate();
    const [subscriberAgreementShortName, setSubscriberAgreementShortName] = React.useState<string>("");
    const [addApiError, setAddApiError] = useState<any>({});

    const [subscriberCredential, setSubscriberCredential] =
        useState<SubscriberCredentialView>(new SubscriberCredentialView(Guid.create()));

    const addSubscriberCredential = subscriberCredentialViewService.useCreateSubscriberCredential();

    const handleAddNew = () => {
        subscriberCredential.supplierSharingAgreementShortName = subscriberAgreementShortName;
        addSubscriberCredential.mutate(subscriberCredential, {
            onSuccess: () => {
                navigate('/subscriberAgreements'); 
            },
            onError: (error: any) => {
                setAddApiError(error?.response?.data?.errors);
            }
        });
    };

    return (
        <CardBase>
            <CardBaseBody>
                <CardBaseTitle>
                    Subscriber Agreements
                </CardBaseTitle>
                <CardBaseContent>
                    <Form>

                        <TextInputBase
                            id="SubscriberAgreementShortName"
                            name="SubscriberAgreementShortName"
                            label="Subscriber Agreement Short Name"
                            placeholder="Subscriber Agreement Short Name"
                            value={subscriberAgreementShortName}
                            onChange={(e) => { setSubscriberAgreementShortName(e.target.value) }}
                        />


                        <br />
                        <ButtonBase onClick={handleAddNew} info>
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