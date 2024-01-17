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
import { SpecificationObjectView } from "../../models/views/components/specificationObjects/specificationObjectView";
import { specificationObjectErrors } from "./specificationObjectErrors";
import { specificationObjectValidations } from "./specificationObjectValidations";
import TextAreaInputBase from "../bases/inputs/TextAreaInputBase";
import CheckboxBase from "../bases/inputs/CheckboxBase";


interface SpecificationObjectDetailCardEditProps {
    specificationObject: SpecificationObjectView;
    onAdd: (specificationObject: SpecificationObjectView) => void;
    onUpdate: (specificationObject: SpecificationObjectView) => void;
    onCancel: () => void;
    mode: string;
    onModeChange: (value: string) => void;
    apiError?: any;
}

const SpecificationObjectDetailCardEdit: FunctionComponent<SpecificationObjectDetailCardEditProps> = (props) => {
    const {
        specificationObject,
        onAdd,
        onUpdate,
        onCancel,
        mode,
        onModeChange,
        apiError
    } = props;

    const [editSpecificationObject, setEditSpecificationObject] = useState<SpecificationObjectView>({ ...specificationObject });

    const { errors, enableValidationMessages, processApiErrors, validate } = useValidation(specificationObjectErrors, specificationObjectValidations, editSpecificationObject)

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement> | ChangeEvent<HTMLSelectElement>) => {
        const updatedSpecificationObject = {
            ...editSpecificationObject,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setEditSpecificationObject(updatedSpecificationObject);
    };

    const handleCancel = () => {
        setEditSpecificationObject({ ...specificationObject });
        onModeChange('VIEW')
        onCancel();
    };

    const handleSave = () => {
        if (!validate(editSpecificationObject)) {
            onAdd(editSpecificationObject)
        } else {
            enableValidationMessages();
        }
    }

    const handleUpdate = () => {
        if (!validate(editSpecificationObject)) {
            onUpdate(editSpecificationObject)
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
                {/*<strong>DataSetId - </strong>{dataSetId}*/}
                <br /><br />
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Object Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="supplierObjectName"
                            name="supplierObjectName"
                            placeholder="Supplier Object Name"
                            required={true}
                            value={editSpecificationObject.supplierObjectName}
                            error={errors.supplierObjectName}
                            onChange={handleChange}/>
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Our Object Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="ourObjectName"
                            name="ourObjectName"
                            placeholder="Our Object Name"
                            required={true}
                            value={editSpecificationObject.ourObjectName}
                            error={errors.ourObjectName}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Object Description</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextAreaInputBase
                            id="objectDescription"
                            name="objectDescription"
                            placeholder="Object Description"
                            rows={5}
                            value={editSpecificationObject.objectDescription}
                            error={errors.objectDescription}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Interchange Protocol</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="interchangeProtocol"
                            name="interchangeProtocol"
                            placeholder="Interchange Protocol"
                            value={editSpecificationObject.interchangeProtocol}
                            error={errors.interchangeProtocol}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Pushed To Us</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isPushedToUs"
                            name="isPushedToUs"
                            label=""
                            checked={editSpecificationObject.isPushedToUs}
                            error={errors.isPushedToUs}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>


                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Pulled By Us</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isPulledByUs"
                            name="isPulledByUs"
                            label=""
                            checked={editSpecificationObject.isPulledByUs}
                            error={errors.isPulledByUs}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Deletion Handling</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="deletionHandling"
                            name="deletionHandling"
                            placeholder="Deletion Handling"
                            value={editSpecificationObject.deletionHandling}
                            error={errors.deletionHandling}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Submission Header Object</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isSubmissionHeaderObject"
                            name="isSubmissionHeaderObject"
                            label=""
                            required={true}
                            checked={editSpecificationObject.isSubmissionHeaderObject}
                            error={errors.isSubmissionHeaderObject}
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Transaction Log</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isTransactionLog"
                            name="isTransactionLog"
                            label=""
                            required={true}
                            checked={editSpecificationObject.isTransactionLog}
                            error={errors.isTransactionLog}
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
