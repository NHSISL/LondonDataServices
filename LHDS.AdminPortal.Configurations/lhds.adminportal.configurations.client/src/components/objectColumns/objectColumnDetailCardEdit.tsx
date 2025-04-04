import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import { SecuredComponents } from "../links";
import securityPoints from "../../securityMatrix";
import { useValidation } from "../../hooks/useValidation";
import TextInputBase from "../bases/inputs/TextInputBase";
import TextAreaInputBase from "../bases/inputs/TextAreaInputBase";
import { ObjectColumnView } from "../../models/views/components/objectColumns/objectColumnView";
import { objectColumnErrors } from "./objectColumnErrors";
import { objectColumnValidations } from "./objectColumnValidations";
import CheckboxBase from "../bases/inputs/CheckboxBase";

interface ObjectColumnDetailCardEditProps {
    objectColumn: ObjectColumnView;
    onAdd: (objectColumn: ObjectColumnView) => void;
    onUpdate: (objectColumn: ObjectColumnView) => void;
    onCancel: () => void;
    mode: string;
    onModeChange: (value: string) => void;
    apiError?: any;
}

const SpecificationObjectDetailCardEdit: FunctionComponent<ObjectColumnDetailCardEditProps> = (props) => {
    const {
        objectColumn,
        onAdd,
        onUpdate,
        onCancel,
        mode,
        onModeChange,
        apiError
    } = props;

    const [editObjectColumn, setEditObjectColumn] = useState<ObjectColumnView>({ ...objectColumn });

    const { errors, enableValidationMessages, processApiErrors, validate } = useValidation(objectColumnErrors, objectColumnValidations, editObjectColumn)

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement> | ChangeEvent<HTMLSelectElement>) => {
        const updatedObjectColumn = {
            ...editObjectColumn,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setEditObjectColumn(updatedObjectColumn);
    };

    const handleCancel = () => {
        setEditObjectColumn({ ...objectColumn });
        onModeChange('VIEW')
        onCancel();
    };

    const handleSave = () => {
        if (!validate(editObjectColumn)) {
            onAdd(editObjectColumn)
        } else {
            enableValidationMessages();
        }
    }

    const handleUpdate = () => {
        if (!validate(editObjectColumn)) {
            onUpdate(editObjectColumn)
        } else {
            enableValidationMessages();
        }
    }

    useEffect(() => {
        processApiErrors(apiError)
    }, [apiError, processApiErrors])

    return (
        <>
            <SummaryListBase>
                {/*<strong>DataSetId - </strong>{objectColumn.DataSetObjectId}*/} 
                <br /><br />
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Column Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="supplierColumnName"
                            name="supplierColumnName"
                            placeholder="Supplier Column Name"
                            required={true}
                            value={editObjectColumn.supplierColumnName}
                            error={errors.supplierColumnName}
                            onChange={handleChange}/>
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Our Column Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="ourColumnName"
                            name="ourColumnName"
                            placeholder="Our Column Name"
                            required={true}
                            value={editObjectColumn.ourColumnName}
                            error={errors.ourColumnName}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Column Description</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextAreaInputBase
                            id="columnDescription"
                            name="columnDescription"
                            placeholder="Object Description"
                            rows={5}
                            value={editObjectColumn.columnDescription}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Ordinal Position</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="ordinalPosition"
                            name="ordinalPosition"
                            placeholder="Ordinal Position"
                            required={true}
                            value={editObjectColumn.ordinalPosition}
                            type="number"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Populated By</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="populatedBy"
                            name="populatedBy"
                            placeholder="Populated By"
                            required={true}
                            value={editObjectColumn.populatedBy}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>FHIR Data Type</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="fhirDataType"
                            name="fhirDataType"
                            placeholder="FHIR Data Type"
                            required={true}
                            value={editObjectColumn.fhirDataType}
                            error={errors.fhirDataType}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>SQL Data Type</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="sqlDataType"
                            name="sqlDataType"
                            placeholder="SQL Data Type"
                            required={true}
                            value={editObjectColumn.sqlDataType}
                            error={errors.sqlDataType}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Length</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="length"
                            name="length"
                            placeholder="Length"
                            required={true}
                            value={editObjectColumn.length}
                            type="number"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Precision</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="precision"
                            name="precision"
                            placeholder="Precision"
                            required={true}
                            value={editObjectColumn.precision}
                            type="number"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Scale</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="scale"
                            name="scale"
                            placeholder="Scale"
                            required={true}
                            value={editObjectColumn.scale}
                            type="number"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Date Format</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="supplierDateFormat"
                            name="supplierDateFormat"
                            placeholder="Supplier Date Format"
                            required={true}
                            value={editObjectColumn.supplierDateFormat}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Watermark</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isWatermark"
                            name="isWatermark"
                            label=""
                            checked={editObjectColumn.isWatermark}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Sequencing</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isSequencing"
                            name="isSequencing"
                            label=""
                            checked={editObjectColumn.isSequencing}
                            error={errors.isSequencing}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Business Key</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isBusinessKey"
                            name="isBusinessKey"
                            label=""
                            checked={editObjectColumn.isBusinessKey}
                            error={errors.isBusinessKey}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Unique Record Key</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isUniqueRecordKey"
                            name="isUniqueRecordKey"
                            label=""
                            checked={editObjectColumn.isUniqueRecordKey}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Version Hash Element</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isVersionHashElement"
                            name="isVersionHashElement"
                            label=""
                            checked={editObjectColumn.isVersionHashElement}
                            error={errors.isVersionHashElement}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Sender Code</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isSenderCode"
                            name="isSenderCode"
                            label=""
                            checked={editObjectColumn.isSenderCode}
                            error={errors.isSenderCode}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Author Code</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isAuthorCode"
                            name="isAuthorCode"
                            label=""
                            checked={editObjectColumn.isAuthorCode}
                            error={errors.isAuthorCode}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Related Organisation Id</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isRelatedOrganisationId"
                            name="isRelatedOrganisationId"
                            label=""
                            checked={editObjectColumn.isRelatedOrganisationId}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Delete Flag</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isDeleteFlag"
                            name="isDeleteFlag"
                            label=""
                            checked={editObjectColumn.isDeleteFlag}
                            error={errors.isDeleteFlag}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Person Confidential Data</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isPersonConfidentialData"
                            name="isPersonConfidentialData"
                            label=""
                            checked={editObjectColumn.isPersonConfidentialData}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Person Confidential DataType</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="personConfidentialDataType"
                            name="personConfidentialDataType"
                            placeholder="Person Confidential DataType"
                            required={true}
                            value={editObjectColumn.personConfidentialDataType}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Masking Method</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="maskingMethod"
                            name="maskingMethod"
                            placeholder="Masking Method"
                            required={true}
                            value={editObjectColumn.maskingMethod}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Sensitive Record Marker</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isSensitiveRecordMarker"
                            name="isSensitiveRecordMarker"
                            label=""
                            checked={editObjectColumn.isSensitiveRecordMarker}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Code System</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="codeSystem"
                            name="codeSystem"
                            placeholder="Code System"
                            required={true}
                            value={editObjectColumn.codeSystem}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Partition Column Level</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="partitionColumnLevel"
                            name="partitionColumnLevel"
                            placeholder="Partition Column Level"
                            required={true}
                            value={editObjectColumn.partitionColumnLevel}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

            </SummaryListBase>

            {mode === "ADD" && (
                <div>
                    {/*<Link to={'/configuration/dataSet/' + dataSetId}>*/}
                    {/*    <ButtonBase onClick={() => { }} add>Cancel</ButtonBase>*/}
                    {/*</Link>*/}
                    <SecuredComponents allowedRoles={securityPoints.dataSets.add}>
                        <ButtonBase onClick={handleSave} add>Add</ButtonBase>
                    </SecuredComponents>
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

export default SpecificationObjectDetailCardEdit;
