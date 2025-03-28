import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import { SecuredComponents } from "../links";
import securityPoints from "../../securityMatrix";
import { useValidation } from "../../hooks/useValidation";
import { Link } from "react-router-dom";
import TextInputBase from "../bases/inputs/TextInputBase";
import { ResolvedAddressView } from "../../models/views/components/resolvedAddresses/resolvedAddressView";
import { resolvedAddressErrors } from "./resolvedAddressSetErrors";
import { resolvedAddressValidation } from "./resolvedAddressValidation";

interface ResolvedAddressDetailCardEditProps {
    resolvedAddress: ResolvedAddressView;
    onUpdate: (resolvedAddresstaSet: ResolvedAddressView) => void;
    onCancel: () => void;
    mode: string;
    onModeChange: (value: string) => void;
    apiError?: any;
    onPickAlternateAddress: (alternateAddress: string) => void;
}

const ResolvedAddressDetailCardEdit: FunctionComponent<ResolvedAddressDetailCardEditProps> = (props) => {
    const {
        resolvedAddress,
        onUpdate,
        onCancel,
        mode,
        onModeChange,
        apiError,
        onPickAlternateAddress
    } = props;

    const [editResolvedAddress, setEditResolvedAddress] = useState<ResolvedAddressView>({ ...resolvedAddress });

    const { errors, enableValidationMessages, processApiErrors, validate } =
        useValidation(resolvedAddressErrors,
            resolvedAddressValidation,
            editResolvedAddress)

    const handleChange = (
        event: ChangeEvent<HTMLInputElement>
            | ChangeEvent<HTMLTextAreaElement>
            | ChangeEvent<HTMLSelectElement>
    ) => {
        const { name, value } = event.target;
        setEditResolvedAddress(prevState => ({
            ...prevState,
            [name]: value,
        }));
    };

    const handleCancel = () => {
        setEditResolvedAddress({ ...resolvedAddress });
        onModeChange('VIEW')
        onCancel();
    };
    const handleUpdate = () => {
        if (!validate(editResolvedAddress)) {
            onUpdate(editResolvedAddress)
        } else {
            enableValidationMessages();
        }
    }

    useEffect(() => {
        setEditResolvedAddress({ ...resolvedAddress });
    }, [resolvedAddress]);

    useEffect(() => {
        processApiErrors(apiError)
    }, [apiError, processApiErrors])

    return (
        <>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Unstructured Address</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <SummaryListBaseValue>{resolvedAddress.unstructuredPostalAddress}</SummaryListBaseValue>
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Alternate Unstructured Address</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <SummaryListBaseValue>
                            {resolvedAddress.alternateUnstructuredPostalAddress ? resolvedAddress.alternateUnstructuredPostalAddress : "Not Specified"}</SummaryListBaseValue>
                    </SummaryListBaseValue>
                </SummaryListBaseRow>
                {/*<SummaryListBaseRow>*/}
                {/*    <SummaryListBaseKey>Alternate Unstructured Address</SummaryListBaseKey>*/}
                {/*    <SummaryListBaseValue>*/}
                {/*        <TextInputBase*/}
                {/*            id="alternateUnstructuredAddress"*/}
                {/*            name="alternateUnstructuredAddress"*/}
                {/*            placeholder="Alternate Unstructured Address"*/}
                {/*            required={false}*/}
                {/*            value={editResolvedAddress.alternateUnstructuredPostalAddress}*/}
                {/*            error={errors.alternateUnstructuredPostalAddress}*/}
                {/*            onChange={handleChange}/>*/}
                {/*    </SummaryListBaseValue>*/}
                {/*</SummaryListBaseRow>*/}
            </SummaryListBase>

            {mode === "ADD" && (
                <div>
                    <Link to={'/configuration/resolvedAddress/'}>
                        <ButtonBase onClick={() => { }} add>Cancel</ButtonBase>
                    </Link>
                </div>
            )}

            {mode !== "ADD" && (
                <div>
                    <ButtonBase onClick={() => handleCancel()} cancel>Cancel</ButtonBase>
                    <SecuredComponents allowedRoles={securityPoints.dataSets.edit}>
                        <ButtonBase onClick={handleUpdate} edit>Update</ButtonBase>
                    </SecuredComponents>
                </div>
            )}
        </>
    );
}

export default ResolvedAddressDetailCardEdit;