import React, { FunctionComponent } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import { SecuredComponents } from "../links";
import securityPoints from "../../securityMatrix";
import { DataSetSpecificationView } from "../../models/views/components/dataSetSpecifications/dataSetSpecificationView";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import moment from "moment";

interface DataSetSpecificationDetailCardViewProps {
    dataSetSpecification: DataSetSpecificationView;
    onDelete: (dataSetSpecification: DataSetSpecificationView) => void;
    mode: string;
    onModeChange: (value: string) => void;
}

const DataSetSpecificationDetailCardView: FunctionComponent<DataSetSpecificationDetailCardViewProps> = (props) => {
    const {
        dataSetSpecification,
        onDelete,
        mode,
        onModeChange
    } = props;


    return (
        <>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Specification Version</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSetSpecification.supplierSpecificationVersion}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Our Specification Version</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSetSpecification.ourSpecificationVersion}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Notes</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSetSpecification.notes}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Multi Sender</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSetSpecification.isMultiAuthorPerBatch ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Entity Change Synchronisation</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSetSpecification.entityChangeSynchronisation}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Date Released</SummaryListBaseKey>
                    <SummaryListBaseValue>{moment(dataSetSpecification.dateReleased?.toString()).format("Do-MMM-yyyy")}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Date Implemented</SummaryListBaseKey>
                    <SummaryListBaseValue>{moment(dataSetSpecification.dateImplementation?.toString()).format("Do-MMM-yyyy")}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Date Superseded</SummaryListBaseKey>
                    <SummaryListBaseValue>{moment(dataSetSpecification.dateSuperseded?.toString()).format("Do-MMM-yyyy")}</SummaryListBaseValue>
                </SummaryListBaseRow>

                {/*<SummaryListBaseRow>*/}
                {/*    <SummaryListBaseKey>Superseded By Id</SummaryListBaseKey>*/}
                {/*    <SummaryListBaseValue>{dataSetSpecification.supersededById?.toString()}</SummaryListBaseValue>*/}
                {/*</SummaryListBaseRow>*/}

                {/*<SummaryListBaseRow>*/}
                {/*    <SummaryListBaseKey>Preceded By Id</SummaryListBaseKey>*/}
                {/*    <SummaryListBaseValue>{dataSetSpecification.presededById!.toString()}</SummaryListBaseValue>*/}
                {/*</SummaryListBaseRow>*/}

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Published</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSetSpecification.isPublished ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Active</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSetSpecification.isActive ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Active From</SummaryListBaseKey>
                    <SummaryListBaseValue>{moment(dataSetSpecification.activeFrom?.toString()).format("Do-MMM-yyyy")}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Active To</SummaryListBaseKey>
                    <SummaryListBaseValue>{moment(dataSetSpecification.activeTo?.toString()).format("Do-MMM-yyyy")}</SummaryListBaseValue>
                </SummaryListBaseRow>

                
            </SummaryListBase>

            {mode === 'VIEW' &&
                <SecuredComponents allowedRoles={securityPoints.dataSets.edit}>
                    <ButtonBase onClick={() => onModeChange('EDIT')} edit>Edit</ButtonBase>
                </SecuredComponents>
            }
            <>
                {mode === 'VIEW' &&
                    <SecuredComponents allowedRoles={securityPoints.dataSets.delete}>
                        <ButtonBase onClick={() => onModeChange('CONFIRMDELETE')} remove>Delete</ButtonBase>
                    </SecuredComponents>
                }
                {mode === 'CONFIRMDELETE' &&
                    <>
                        <SecuredComponents allowedRoles={securityPoints.dataSets.delete}>
                            <>
                                <ButtonBase onClick={() => onModeChange('VIEW')} cancel>Cancel</ButtonBase>
                                <ButtonBase onClick={() => onDelete(dataSetSpecification)} remove>Yes, Delete</ButtonBase>
                            </>
                        </SecuredComponents>
                    </>
                }
            </>
        </>
    );
}

export default DataSetSpecificationDetailCardView;
