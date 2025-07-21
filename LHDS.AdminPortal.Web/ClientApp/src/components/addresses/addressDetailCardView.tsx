import React, { FunctionComponent } from "react";
import moment from "moment";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import GridBase from "../bases/layouts/Grid/GridBase";
import { AddressView } from "../../models/views/components/addresses/addressView";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";

interface AddressDetailCardViewProps {
    address: AddressView;
    onRefresh: (address: AddressView) => void;
}

const AddressDetailCardView: FunctionComponent<AddressDetailCardViewProps> = (props) => {
    const {
        address
    } = props;


    return (
        <>
            <div className="row">
                <GridBase>
                    <CardBase>
                        <CardBaseBody>
                            <CardBaseTitle>
                                Details
                            </CardBaseTitle>
                            <CardBaseContent>
                                <SummaryListBase>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>UPRN</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.uprn}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>USRN</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.usrn}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Organisation Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.organisationName}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Department Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.departmentName}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Sub-Building Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.subBuildingName}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Building Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.buildingName}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Building Number</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.BuildingNumber}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Dependent Thoroughfare</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.dependentThoroughfare}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Thoroughfare</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.thoroughfare}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Double Dependen Locality</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.doubleDependentLocality}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Post Town</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.postTown}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Post Code</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.postCode}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>isProcessing</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.isProcessing ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>isSynced</SummaryListBaseKey>
                                        <SummaryListBaseValue>{address.isSynced ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                     <SummaryListBaseRow>
                                        <SummaryListBaseKey>Created Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(address.createdDate?.toString())
                                                .format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Updated Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(address.updatedDate?.toString())
                                                .format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                </SummaryListBase>
                            </CardBaseContent>
                        </CardBaseBody>
                    </CardBase>
                </GridBase>
            </div>
        </>
    );
}

export default AddressDetailCardView;