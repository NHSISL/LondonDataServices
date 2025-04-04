import React, { FunctionComponent } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import { SecuredComponents } from "../links";
import securityPoints from "../../securityMatrix";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import { ObjectColumnView } from "../../models/views/components/objectColumns/objectColumnView";

interface ObjectColumnDetailCardViewProps {
    objectColumn: ObjectColumnView;
    onDelete: (objectColumn: ObjectColumnView) => void;
    mode: string;
    onModeChange: (value: string) => void;
}

const ObjectColumnDetailCardView: FunctionComponent<ObjectColumnDetailCardViewProps> = (props) => {
    const {
        objectColumn,
        onDelete,
        mode,
        onModeChange
    } = props;

    return (
        <>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Column Name</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.supplierColumnName}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Our Column Name</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.ourColumnName}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Column Description</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.columnDescription}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Ordinal Position</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.ordinalPosition}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Populated By</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.populatedBy}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>FHIR Data Type</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.fhirDataType}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>SQL Data Type</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.sqlDataType}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Length</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.length}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Precision</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.precision}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Scale</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.scale}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Date Format</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.supplierDateFormat}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Watermark</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isWatermark ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Sequencing</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isSequencing ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Business Key</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isBusinessKey ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Unique Record Key</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isUniqueRecordKey ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Version Hash Element</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isVersionHashElement ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Sender Code</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isSenderCode ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Author Code</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isAuthorCode ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Related Organisation Id</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isRelatedOrganisationId ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Delete Flag</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isDeleteFlag ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Person Confidential Data</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isPersonConfidentialData ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Person Confidential DataType</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.personConfidentialDataType}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Masking Method</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.maskingMethod}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Sensitive Record Marker</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.isSensitiveRecordMarker ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Code System</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.codeSystem}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Partition Column Level</SummaryListBaseKey>
                    <SummaryListBaseValue>{objectColumn.partitionColumnLevel}</SummaryListBaseValue>
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
                            <ButtonBase onClick={() => onDelete(objectColumn)} remove>Yes, Delete</ButtonBase>
                            </>
                        </SecuredComponents>
                    </>
                }
            </>
        </>
    );
}

export default ObjectColumnDetailCardView;
