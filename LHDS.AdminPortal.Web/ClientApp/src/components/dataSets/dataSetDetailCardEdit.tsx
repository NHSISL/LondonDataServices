import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import SelectInputBase from "../bases/inputs/SelectInputBase";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import SummaryListBaseAction from "../bases/components/SummaryList/SummaryListBase.Action";
import { SecuredComponents } from "../Links";
import securityPoints from "../../SecurityMatrix";
import { useValidation } from "../../hooks/useValidation";
import { Link } from "react-router-dom";
import { DataSetView } from "../../models/views/components/dataSets/dataSetView";
import TextInputBase from "../bases/inputs/TextInputBase";
import { dataSetErrors } from "./dataSetErrors";
import { dataSetValidation } from "./dataSetValidation";
import CheckboxBase from "../bases/inputs/CheckboxBase";
import DateInputBase from "../bases/inputs/DateInputBase";
import moment from "moment";

interface DataSetDetailCardEditProps {
    dataSet: DataSetView;
    onAdd: (dataSet: DataSetView) => void;
    onUpdate: (dataSet: DataSetView) => void;
    onCancel: () => void;
    mode: string;
    onModeChange: (value: string) => void;
    apiError?: any;
}

const DataSetDetailCardEdit: FunctionComponent<DataSetDetailCardEditProps> = (props) => {
    const {
        dataSet,
        onAdd,
        onUpdate,
        onCancel,
        mode,
        onModeChange,
        apiError
    } = props;

    const [editDataSet, setEditDataSet] = useState<DataSetView>({ ...dataSet });

    const { errors, enableValidationMessages, processApiErrors, validate } = useValidation(dataSetErrors, dataSetValidation, editDataSet)

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement> | ChangeEvent<HTMLSelectElement>) => {
        const updatedDataSet = {
            ...editDataSet,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setEditDataSet(updatedDataSet);
    };

    const handleCancel = () => {
        setEditDataSet({ ...dataSet });
        onModeChange('VIEW')
        onCancel();
    };

    const handleSave = () => {
        if (!validate(editDataSet)) {
            onAdd(editDataSet)
        } else {
            enableValidationMessages();
        }
    }

    const handleUpdate = () => {
        if (!validate(editDataSet)) {
            onUpdate(editDataSet)
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
                    <SummaryListBaseKey>DataSet Name</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="dataSetName"
                            name="dataSetName"
                            placeholder="DataSet Name"
                            value={editDataSet.dataSetName}
                            error={errors.dataSetName}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>

                    <SummaryListBaseAction> </SummaryListBaseAction>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>DataSet Aliases</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="DataSetAliasses"
                            name="dataSetAliasses"
                            placeholder="DataSet Aliasses"
                            value={editDataSet.dataSetAliasses}
                            error={errors.dataSetAliasses}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                    <SummaryListBaseAction> </SummaryListBaseAction>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>DataSet Supplier</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="dataSetSupplier"
                            name="dataSetSupplier"
                            placeholder="DataSet Supplier"
                            value={editDataSet.dataSetSupplier}
                            error={errors.dataSetSupplier}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                    <SummaryListBaseAction> </SummaryListBaseAction>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>DataSet Author</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="dataSetAuthor"
                            name="dataSetAuthor"
                            placeholder="DataSet Author"
                            value={editDataSet.dataSetAuthor}
                            error={errors.dataSetAuthor}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                    <SummaryListBaseAction> </SummaryListBaseAction>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Data Source Type</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <TextInputBase
                            id="dataSourceType"
                            name="dataSourceType"
                            placeholder="Data Source Type"
                            value={editDataSet.dataSourceType}
                            error={errors.dataSourceType}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                    <SummaryListBaseAction> </SummaryListBaseAction>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Active</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <CheckboxBase
                            id="isActive"
                            name="isActive"
                            label=""
                            checked={editDataSet.isActive}
                            error={errors.isActive}
                            onChange={handleChange}
                        />
                    </SummaryListBaseValue>
                    <SummaryListBaseAction></SummaryListBaseAction>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Active From</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <DateInputBase
                            id="activeFrom"
                            name="activeFrom"
                            placeholder="Active From Date"
                            value={editDataSet.activeFrom ? moment(editDataSet?.activeFrom).format("YYYY-MM-DD") : ""}
                            required={true}
                            error={errors.activeFrom}
                            type="date"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                    <SummaryListBaseAction></SummaryListBaseAction>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Active To</SummaryListBaseKey>
                    <SummaryListBaseValue>
                        <DateInputBase
                            id="activeTo"
                            name="activeTo"
                            placeholder="Active To Date"
                            value={editDataSet.activeTo ? moment(editDataSet?.activeTo).format("YYYY-MM-DD") : ""}
                            required={true}
                            error={errors.activeTo}
                            type="date"
                            onChange={handleChange} />
                    </SummaryListBaseValue>
                    <SummaryListBaseAction></SummaryListBaseAction>
                </SummaryListBaseRow>
            </SummaryListBase>

            {mode === "ADD" && (
                <div>
                    <Link to={'/configuration/dataSets/'}>
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

export default DataSetDetailCardEdit;
