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
import { dataSetSpecificationErrors } from "./dataSetSpecificationErrors";
import { dataSetSpecificationValidation } from "./dataSetSpecificationValidation";
import { DataSetSpecificationView } from "../../models/views/components/dataSetSpecifications/dataSetSpecificationView";
import TextAreaInputBase from "../bases/inputs/TextAreaInputBase";
import CheckboxBase from "../bases/inputs/CheckboxBase";
import DateInputBase from "../bases/inputs/DateInputBase";
import moment from "moment";

interface DataSetSpecificationDetailCardEditProps {
    dataSetSpecification: DataSetSpecificationView;
    dataSetId: string;
    onAdd: (dataSetSpecification: DataSetSpecificationView) => void;
    onUpdate: (dataSetSpecification: DataSetSpecificationView) => void;
    onCancel: () => void;
    mode: string;
    onModeChange: (value: string) => void;
    apiError?: any;
}

const DataSetSpecificationDetailCardEdit: FunctionComponent<DataSetSpecificationDetailCardEditProps> = (props) => {
    const {
        dataSetSpecification,
        dataSetId,
        onAdd,
        onUpdate,
        onCancel,
        mode,
        onModeChange,
        apiError
    } = props;

    const [editDataSetSpecification, setEditDataSetSpecification] = useState<DataSetSpecificationView>({ ...dataSetSpecification });

    const { errors, enableValidationMessages, processApiErrors, validate } = useValidation(dataSetSpecificationErrors, dataSetSpecificationValidation, editDataSetSpecification)

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement> | ChangeEvent<HTMLSelectElement>) => {
        const updatedDataSetSpecification = {
            ...editDataSetSpecification,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setEditDataSetSpecification(updatedDataSetSpecification);
    };

    const handleCancel = () => {
        setEditDataSetSpecification({ ...dataSetSpecification });
        onModeChange('VIEW')
        onCancel();
    };

    const handleSave = () => {
        if (!validate(editDataSetSpecification)) {
            onAdd(editDataSetSpecification)
        } else {
            enableValidationMessages();
        }
    }

    const handleUpdate = () => {
        if (!validate(editDataSetSpecification)) {
            onUpdate(editDataSetSpecification)
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
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Specification Version</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="supplierSpecificationVersion "
                            name="supplierSpecificationVersion"
                            placeholder="Supplier Specification Version"
                            required={true}
                            value={editDataSetSpecification.supplierSpecificationVersion}
                            error={errors.supplierSpecificationVersion}
                            onChange={handleChange}/>
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Our Specification Version</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="ourSpecificationVersion"
                            name="ourSpecificationVersion"
                            placeholder="Our Specification Version"
                            required={true}
                            value={editDataSetSpecification.ourSpecificationVersion}
                            error={errors.ourSpecificationVersion}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Notes</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextAreaInputBase
                            id="notes"
                            name="notes"
                            placeholder="Notes"
                            rows={5}
                            required={true}
                            value={editDataSetSpecification.notes}
                            error={errors.notes}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Multi Author Per Batch </SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isMultiAuthorPerBatch"
                            name="isMultiAuthorPerBatch"
                            label=""
                            checked={editDataSetSpecification.isMultiAuthorPerBatch}
                            error={errors.isMultiAuthorPerBatch}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Entity Change Synchronisation</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextAreaInputBase
                            id="entityChangeSynchronisation"
                            name="entityChangeSynchronisation"
                            placeholder="Entity Change Synchronisation"
                            rows={2}
                            required={true}
                            value={editDataSetSpecification.entityChangeSynchronisation}
                            error={errors.notes}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>


                <SummaryListBaseRow>
                    <SummaryListBaseKey>Date Released </SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <DateInputBase
                            id="dateReleased"
                            name="dateReleased"
                            placeholder="Date Released"
                            value={editDataSetSpecification.dateReleased ? moment(editDataSetSpecification?.dateReleased).format("YYYY-MM-DD") : ""}
                            error={errors.dateReleased}
                            type="date"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>DateImplementation</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <DateInputBase
                            id="dateImplementation"
                            name="dateImplementation"
                            placeholder="Date Implementation"
                            value={editDataSetSpecification.dateImplementation ? moment(editDataSetSpecification?.dateImplementation).format("YYYY-MM-DD") : ""}
                            error={errors.dateImplementation}
                            type="date"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Date Superseded</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <DateInputBase
                            id="dateSuperseded"
                            name="dateSuperseded"
                            placeholder="Date Superseded"
                            value={editDataSetSpecification.dateSuperseded ? moment(editDataSetSpecification?.dateSuperseded).format("YYYY-MM-DD") : ""}
                            error={errors.dateSupersed}
                            type="date"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>


                <SummaryListBaseRow>
                    <SummaryListBaseKey>Preceded By</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        PrecededById
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Superseded By</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        Superseded By Id
                    </SummaryListBaseValue>
                </SummaryListBaseRow>


                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Published</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isPublished"
                            name="isPublished"
                            label=""
                            required={true}
                            checked={editDataSetSpecification.isPublished}
                            error={errors.isPublished}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>IsActive</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isActive"
                            name="isActive"
                            label=""
                            required={true}
                            checked={editDataSetSpecification.isActive}
                            error={errors.isActive}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Active From</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <DateInputBase
                            id="activeFrom"
                            name="activeFrom"
                            placeholder="Active From"
                            value={editDataSetSpecification.activeFrom ? moment(editDataSetSpecification?.activeFrom).format("YYYY-MM-DD") : ""}
                            error={errors.activeFrom}
                            type="date"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Active To</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <DateInputBase
                            id="activeTo"
                            name="activeTo"
                            placeholder="Active To"
                            value={editDataSetSpecification.activeTo ? moment(editDataSetSpecification?.activeTo).format("YYYY-MM-DD") : ""}
                            error={errors.activeTo}
                            type="date"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

            </SummaryListBase>

            {mode === "ADD" && (
                <div>
                    <Link to={'/configuration/dataSet/' + dataSetId}>
                        <ButtonBase onClick={() => { }} add>Cancel</ButtonBase>
                    </Link>
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

export default DataSetSpecificationDetailCardEdit;
