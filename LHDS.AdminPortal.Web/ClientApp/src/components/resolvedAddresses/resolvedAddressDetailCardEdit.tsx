import React, { FunctionComponent, useState, useEffect } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import { SecuredComponents } from "../links";
import securityPoints from "../../securityMatrix";
import { Link } from "react-router-dom";
import { ResolvedAddressView } from "../../models/views/components/resolvedAddresses/resolvedAddressView";

interface ResolvedAddressDetailCardEditProps {
    resolvedAddress: ResolvedAddressView;
    onUpdate: (resolvedAddresstaSet: ResolvedAddressView) => void;
    onCancel: () => void;
    mode: string;
    onModeChange: (value: string) => void;
}

const ResolvedAddressDetailCardEdit: FunctionComponent<ResolvedAddressDetailCardEditProps> = (props) => {
    const {
        resolvedAddress,
        onUpdate,
        onCancel,
        mode,
        onModeChange
    } = props;

    const [editResolvedAddress, setEditResolvedAddress] = useState<ResolvedAddressView>({ ...resolvedAddress });

    const handleCancel = () => {
        setEditResolvedAddress({ ...resolvedAddress });
        onModeChange('VIEW')
        onCancel();
    };
    const handleUpdate = () => {
        onUpdate(editResolvedAddress)
    }

    useEffect(() => {
        setEditResolvedAddress({ ...resolvedAddress });
    }, [resolvedAddress]);

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
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                            <span>
                                {editResolvedAddress.alternateUnstructuredPostalAddress ?? "Not Specified"}
                            </span>
                            <ButtonBase
                                onClick={() => setEditResolvedAddress(prev => ({
                                    ...prev,
                                    alternateUnstructuredPostalAddress: null
                                }))}
                                style={{ marginLeft: '1rem' }}
                            >
                                Clear
                            </ButtonBase>
                        </div>
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

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