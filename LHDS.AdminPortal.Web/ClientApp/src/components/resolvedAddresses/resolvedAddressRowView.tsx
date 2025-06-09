import React, { FunctionComponent } from "react";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import ButtonBase from "../bases/buttons/ButtonBase";
import { Link } from "react-router-dom";
import { ResolvedAddress } from "../../models/resolvedAddresses/resolvedAddress";

type ResolvedAddressRowProps = {
    resolvedAddress: ResolvedAddress;
}

const ResolvedAddressRowView: FunctionComponent<ResolvedAddressRowProps> = (props) => {
    const {
        resolvedAddress
    } = props;


    const concateAddresses = (resolvedAddress: ResolvedAddress) => {
        const parts = [
            resolvedAddress.organisationName,
            resolvedAddress.departmentName,
            resolvedAddress.subBuildingName,
            resolvedAddress.buildingName,
            resolvedAddress.buildingNumber,
            resolvedAddress.dependentThoroughfare,
            resolvedAddress.thoroughfare,
            resolvedAddress.doubleDependentLocality,
            resolvedAddress.dependentLocality,
            resolvedAddress.postTown,
            resolvedAddress.postCode

        ];

        return parts.filter(part => part).join(', ');
    }

    return (
        <TableBaseRow>
            <TableBaseData>
                <span>
                    <span style={{ fontSize: '14px' }}>{resolvedAddress.uprn}</span><br />
                </span>
            </TableBaseData>
            <TableBaseData>
                <span>
                    <span style={{ fontSize: '14px' }}>{resolvedAddress.upsn}</span>
                </span>
            </TableBaseData>
            <TableBaseData>
                <span
                    style={{
                        fontSize: '14px',
                        color: resolvedAddress.matchedWithAssign ? undefined : 'red',
                    }}
                >
                    {resolvedAddress.unstructuredPostalAddress}
                </span>

                {resolvedAddress.alternateUnstructuredPostalAddress && (
                    <>
                        <br />
                        <small style={{ color: 'yellow' }}>
                            alternate: {resolvedAddress.alternateUnstructuredPostalAddress}
                        </small>
                    </>
                )}
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: '14px' }}>{concateAddresses(resolvedAddress)}</span>
            </TableBaseData>

            <TableBaseData classes="text-center">
                <span>
                    {resolvedAddress.isExported ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" title="processing" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" title="not processing" />}
                </span>
            </TableBaseData>
            <TableBaseData classes="text-center">
                <span>
                    {resolvedAddress.matchedWithAssign ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" title="matched" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" title="not Matched" />}
                </span>
            </TableBaseData>
            <TableBaseData classes="text-center">
                <Link to={`/resolvedAddressDetail/${resolvedAddress.id}`}>
                    {
                        <ButtonBase onClick={() => { }} add> Details </ButtonBase>
                    }
                </Link>
            </TableBaseData>

        </TableBaseRow>
    );
}

export default ResolvedAddressRowView;