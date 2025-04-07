import React, { FunctionComponent, useMemo, useState} from "react";
import { DataTypeView } from "../../models/views/components/dataTypes/dataTypeView";
import { debounce } from "lodash";
import SearchBase from "../bases/inputs/SearchBase";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseThead from "../bases/components/Table/TableBase.Thead";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";
import DataTypeRowNew from "./dataTypeRowNew";
import DataTypeRowAdd from "./dataTypeRowAdd";
import DataTypeRow from "./dataTypeRow";
import { dataTypeViewService } from "../../services/views/dataTypes/dataTypeViewService";

type DataTypeTableProps = {
    allowedToAdd: boolean;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
}

const DataTypeTable: FunctionComponent<DataTypeTableProps> = (props) => {
    const {
        allowedToAdd,
        allowedToEdit,
        allowedToDelete,
    } = props;

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");

    const { mappedDataTypes: dataTypesRetrieved, isLoading } =
        dataTypeViewService.useGetAllDataTypes(debouncedTerm);

    const addDataType = dataTypeViewService.useCreateDataType();
    const updateDataType = dataTypeViewService.useUpdateDataType();
    const deleteDataType = dataTypeViewService.useRemoveDataType();

    const [addMode, setAddMode] = useState<boolean>(false);

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    }

    const handleAddState = () => {
        setAddMode(!addMode);
    };

    const handleAddNew = async (dataType: DataTypeView) => {
        return addDataType.mutate(dataType, {
            onSuccess: () => {
                setAddMode(false);
            }
        });
    };

    const handleUpdate = (dataType: DataTypeView) => {
        return updateDataType.mutateAsync(dataType);
    }

    const handleDelete = (dataType: DataTypeView) => {
        return deleteDataType.mutateAsync(dataType.id);
    }

    const handleDebounce = useMemo(
        () => debounce((value: string) => {
            setDebouncedTerm(value)
        }, 500)
        , [])

    return (
        <>
            <SearchBase id="search" label="Search Data Types" value={searchTerm}
                onChange={(e) => { handleSearchChange(e.currentTarget.value) }} />
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Data Types
                    </CardBaseTitle>
                    <CardBaseContent>
                        {isLoading && <> <SpinnerBase />.</>}
                        <TableBase>
                            <TableBaseThead>
                                <TableBaseRow>
                                    <TableBaseData>Data Types</TableBaseData>
                                    <TableBaseData></TableBaseData>
                                    <TableBaseData classes="text-end">Action(s)</TableBaseData>
                                </TableBaseRow>
                            </TableBaseThead>
                            <TableBaseTbody>
                                {
                                    allowedToAdd &&
                                    <>
                                        {addMode === false && (<DataTypeRowNew onAdd={handleAddState} />)}

                                        {addMode === true && (
                                            <DataTypeRowAdd
                                                onCancel={handleAddState}
                                                onAdd={handleAddNew}
                                                apiError={addApiError} />)}
                                    </>
                                }

                                {dataTypesRetrieved?.map((dataType: DataTypeView) =>
                                    <DataTypeRow
                                        key={dataType.id?.toString()}
                                        dataType={dataType}
                                        allowedToEdit={allowedToEdit}
                                        allowedToDelete={allowedToDelete}
                                        onUpdate={handleUpdate}
                                        onDelete={handleDelete}
                                    />)}
                            </TableBaseTbody>
                        </TableBase>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </>
    );
}

export default DataTypeTable;