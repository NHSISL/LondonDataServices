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
import { Guid } from "guid-typescript";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import { supplierViewService } from "../../services/views/suppliers/supplierViewService";

interface SubscriberAgreementAddProps {
    children?: React.ReactNode;
}

const SubscriberAgreementAdd: FunctionComponent<SubscriberAgreementAddProps> = () => {

    const navigate = useNavigate();
    const [subscriberAgreementShortName, setSubscriberAgreementShortName] = React.useState<string>("");
    const [setAddApiError] = useState<any>({});
    const [loading, setLoading] = useState<boolean>(false);

    const { mappedSuppliers: suppliersRetrieved, isLoading } =
        supplierViewService.useGetAllSuppliers();

    const [selectedSupplierId, setSelectedSupplierId] = useState<string>("");

    const [subscriberCredential] =
        useState<SubscriberCredentialView>(new SubscriberCredentialView(Guid.create()));

    const addSubscriberCredential = subscriberCredentialViewService.useCreateSubscriberCredential();
    const addSubscriberCredentialNoKeys = subscriberCredentialViewService.useCreateSubscriberCredentialNoKeys();

    const handleAddNew = () => {
        setLoading(true);
        subscriberCredential.supplierSharingAgreementShortName = subscriberAgreementShortName;
        subscriberCredential.supplierId = selectedSupplierId;

        // Find selected supplier object to check its name
        const selectedSupplier = suppliersRetrieved?.find(
            (supplier: any) => supplier.id?.toString() === selectedSupplierId
        );

        const isEmis = selectedSupplier?.name?.toLowerCase() === "emis";

        const mutation = isEmis
            ? addSubscriberCredential
            : addSubscriberCredentialNoKeys;

        mutation.mutate(subscriberCredential, {
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


                            <Form.Group controlId="SupplierSelect">
                                <Form.Label>Supplier</Form.Label>
                                <Form.Select
                                    value={selectedSupplierId}
                                    onChange={e => setSelectedSupplierId(e.target.value)}
                                    disabled={isLoading}
                                >
                                    <option value="">Select a supplier...</option>
                                    {suppliersRetrieved &&
                                        suppliersRetrieved
                                            .filter((supplier: any) => supplier.isIngestionTracked === true || supplier.isIngestionTracked === 1)
                                            .map((supplier: any) => (
                                                <option key={supplier.id} value={supplier.id}>
                                                    {supplier.name}
                                                </option>
                                            ))}
                                </Form.Select>
                            </Form.Group>

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