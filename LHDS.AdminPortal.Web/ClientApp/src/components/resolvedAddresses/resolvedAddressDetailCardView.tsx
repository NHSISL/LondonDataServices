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
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import { ResolvedAddressView } from "../../models/views/components/resolvedAddresses/resolvedAddressView";
import { SecuredComponents } from "../links";
import ButtonBase from "../bases/buttons/ButtonBase";
import securityPoints from "../../securityMatrix";

interface ResolvedAddressDetailCardViewProps {
    resolvedAddress: ResolvedAddressView;
    onRefresh: (resolvedAddress: ResolvedAddressView) => void;
    onUpdate: (resolvedAddress: ResolvedAddressView) => void;
    mode: string;
    onModeChange: (value: string) => void;
}

const ResolvedAddressDetailCardView: FunctionComponent<ResolvedAddressDetailCardViewProps> = (props) => {
    const {
        resolvedAddress,
        onUpdate,
        mode,
        onModeChange
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
                                        <SummaryListBaseKey>Unique Reference</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.uniqueReference}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Batch References</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.batchReference}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>UPRN</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.uprn}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>UPSN</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.upsn}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Unstructured Postal Address</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.unstructuredPostalAddress}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                   
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Address Format Quality</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.addressFormatQuality}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Algorithm</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.algorithm}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Building Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.buildingName}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Building Number</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.buildingName}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Classification</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.classification}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Department Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.departmentName}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Dependant Locality</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.dependentLocality}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Dependent Thoroughfare</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.dependentThoroughfare}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Double Dependendant Locality</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.doubleDependentLocality}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Match Pattern</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.matchPattern}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Match With Assign</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.matchedWithAssign ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Organisation Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.organisationName}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>PostCode Quality</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.postCodeQuality}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Post Town</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.postTown}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Qualifier</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.qualifier}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Sub-Building Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.subBuildingName}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Thoroughfare</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.thoroughfare}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Post Code</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.postCode}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>isExported</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.isExported ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>isProcessed</SummaryListBaseKey>
                                        <SummaryListBaseValue>{resolvedAddress.isProcessed ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                     <SummaryListBaseRow>
                                        <SummaryListBaseKey>Created Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(resolvedAddress.createdDate?.toString())
                                                .format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Updated Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(resolvedAddress.updatedDate?.toString())
                                                .format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                </SummaryListBase>
                                <SecuredComponents allowedRoles={securityPoints.resolvedAddress.edit}>
                                    <ButtonBase onClick={() => onModeChange('EDIT')} edit>Edit</ButtonBase>
                                </SecuredComponents>
                            </CardBaseContent>
                        </CardBaseBody>
                    </CardBase>
                </GridBase>
            </div>
        </>
    );
}

export default ResolvedAddressDetailCardView;