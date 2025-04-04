import React, { FunctionComponent, useState } from "react";
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
import { SpinnerBase } from "../bases/spinner/SpinnerBase";

interface SubscriberAgreementAddProps {
    children?: React.ReactNode;
}

const SubscriberAgreementAdd: FunctionComponent<SubscriberAgreementAddProps> = () => {

    const navigate = useNavigate();
    const [subscriberAgreementShortName, setSubscriberAgreementShortName] = React.useState<string>("");
    const [setAddApiError] = useState<any>({});
    const [loading, setLoading] = useState<boolean>(false);

    const [subscriberCredential] =
        useState<SubscriberCredentialView>(new SubscriberCredentialView(crypto.randomUUID()));

    const addSubscriberCredential = subscriberCredentialViewService.useCreateSubscriberCredential();

    const handleAddNew = () => {
        setLoading(true);
        subscriberCredential.supplierSharingAgreementShortName = subscriberAgreementShortName;
        addSubscriberCredential.mutate(subscriberCredential, {
            onSuccess: () => {
                navigate('/subscriberAgreements');
            },
            onError: (error: any) => {
                setAddApiError(error?.response?.data?.errors);
            },
            onSettled: () => {
                setLoading(false);
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
                    {loading ? (
                        <SpinnerBase />
                    ) : (
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
                            <ButtonBase onClick={handleAddNew} add>
                                Create Subscriber Agreement and Generate Keys &nbsp;
                                <FontAwesomeIcon icon={faKey} title="required" />
                            </ButtonBase>

                            <br /><br />
                                <div className="p-3 mb-2 bg-dark text-white">
                                    <strong>NOTE:</strong>
                                    <br />
                                <p>When you click the generate button, our system will begin generating
                                        custom Azure Key Vault secrets for
                                        all the necessary keys required to land data from EMIS.  

                                    This process may take a minute or so,
                                    as we want to ensure each secret is created accurately and securely.
                                </p>
                            </div>
                        </Form>
                    )}
                </CardBaseContent>
            </CardBaseBody>
        </CardBase>
    );
}

export default SubscriberAgreementAdd;