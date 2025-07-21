import React, { FunctionComponent } from "react";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import ButtonBase from "../bases/buttons/ButtonBase";
import { Link } from "react-router-dom";
import { Address } from "../../models/addresses/address";

type AddressRowProps = {
    address: Address;
}

const AddressRowView: FunctionComponent<AddressRowProps> = (props) => {
    const {
        address
    } = props;


    const concateAddresses = (address: Address) => {
        const parts = [
            address.organisationName,
            address.departmentName,
            address.subBuildingName,
            address.buildingName,
            address.buildingNumber,
            address.dependentThoroughfare,
            address.thoroughfare,
            address.doubleDependentLocality,
            address.dependentLocality,
            address.postTown
        ];

        return parts.filter(part => part).join(', ');
    }

    return (
        <TableBaseRow>
            <TableBaseData>
                <span style={{ fontSize: '14px' }}>{address.uprn}</span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: '14px' }}>{address.usrn}</span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: '14px' }}>{concateAddresses(address)}</span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: '14px' }}>{address.postCode}</span>
            </TableBaseData>
            <TableBaseData classes="text-center">
                <span>
                    {address.isProcessing ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" title="processing" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" title="not processing" />}
                </span>
            </TableBaseData>
            <TableBaseData classes="text-center">
                <span>
                    {address.isSynced ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" title="processing" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" title="not processing" />}
                </span>
            </TableBaseData>
            <TableBaseData classes="text-center">
                <Link to={`/addressDetail/${address.id}`}>
                    {
                        <ButtonBase onClick={() => { }} add> Details </ButtonBase>
                    }
                </Link>
            </TableBaseData>

        </TableBaseRow>
    );
}

export default AddressRowView;