import React, { FunctionComponent } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import { SecuredComponents } from "../links";
import securityPoints from "../../securityMatrix";
import { DataSetView } from "../../models/views/components/dataSets/dataSetView";
import moment from "moment";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
interface DataSetDetailCardViewProps {
    dataSet: DataSetView;
    onDelete: (dataSet: DataSetView) => void;
    mode: string;
    onModeChange: (value: string) => void;
}

const DataSetDetailCardView: FunctionComponent<DataSetDetailCardViewProps> = (props) => {
    const {
        dataSet,
        onDelete,
        mode,
        onModeChange
    } = props;


    return (
        <>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>DataSet Name</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSet.dataSetName}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>DataSet Aliases</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSet.dataSetAliases}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>DataSet Supplier</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSet.dataSetSupplier}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>DataSet Author</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSet.dataSetAuthor}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Specified By</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSet.specifiedBy}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Nationally Specified</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSet.IsNationallySpecified ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Collected By</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSet.collectedBy}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Nationally Collected</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSet.isNationallyCollected ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>isActive</SummaryListBaseKey>
                    <SummaryListBaseValue>{dataSet.isActive ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Active From</SummaryListBaseKey>
                    <SummaryListBaseValue>{moment(dataSet.activeFrom?.toString()).format("Do-MMM-yyyy")}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Active To</SummaryListBaseKey>
                    <SummaryListBaseValue>{moment(dataSet.activeTo?.toString()).format("Do-MMM-yyyy")}</SummaryListBaseValue>
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
                                <ButtonBase onClick={() => onDelete(dataSet)} remove>Yes, Delete</ButtonBase>
                            </>
                        </SecuredComponents>
                    </>
                }
            </>
        </>
    );
}

export default DataSetDetailCardView;
