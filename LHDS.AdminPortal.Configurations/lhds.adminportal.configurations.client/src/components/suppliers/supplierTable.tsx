import React, { FunctionComponent, useMemo, useState } from "react";
import SearchBase from "../bases/inputs/SearchBase";
import { debounce } from "lodash";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import SupplierRowNew from "./supplierRowNew";
import SupplierRowAdd from "./supplierRowAdd";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import { supplierViewService } from "../../services/views/suppliers/supplierViewService";
import SupplierRow from "./supplierRow";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import TableBaseThead from "../bases/components/Table/TableBase.Thead";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";

type SupplierTableProps = {
    allowedToAdd: boolean;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
}

const SupplierTable: FunctionComponent<SupplierTableProps> = (props) => {
    const {
        allowedToAdd,
        allowedToEdit,
        allowedToDelete,
    } = props;

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");

    const { mappedSuppliers: suppliersRetrieved, isLoading } =
        supplierViewService.useGetAllSuppliers(debouncedTerm);

    const addSupplier = supplierViewService.useCreateSupplier();
    const updateSupplier = supplierViewService.useUpdateSupplier();
    const deleteSupplier = supplierViewService.useRemoveSupplier();
    const [addMode, setAddMode] = useState<boolean>(false);
    const [addApiError, setAddApiError] = useState<any>({});

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    }

    const handleAddState = () => {
        setAddMode(!addMode);
    };

    const handleAddNew = (supplier: SupplierView) => {
        return addSupplier.mutate(supplier, {
            onSuccess: () => {
                setAddMode(false);
            },
            onError: (error: any) => {
                setAddApiError(error?.response?.data?.errors);
            }
        });
    };

    const handleUpdate = (supplier: SupplierView) => {
        return updateSupplier.mutateAsync(supplier);
    }

    const handleDelete = (supplier: SupplierView) => {
        return deleteSupplier.mutateAsync(supplier.id);
    }

    const handleDebounce = useMemo(
        () => debounce((value: string) => {
            setDebouncedTerm(value)
        }, 500)
        , [])

    return (
        <>
            <SearchBase id="search" label="Search suppliers" value={searchTerm}
                onChange={(e) => { handleSearchChange(e.currentTarget.value) }} />
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Suppliers
                    </CardBaseTitle>
                    <CardBaseContent>
                        {isLoading && <> <SpinnerBase />.</>}
                        <TableBase>
                            <TableBaseThead>
                                <TableBaseRow>
                                    <TableBaseData>Suppliers</TableBaseData>
                                    <TableBaseData></TableBaseData>
                                    <TableBaseData classes="text-end">Action(s)</TableBaseData>
                                </TableBaseRow>
                            </TableBaseThead>
                            <TableBaseTbody>
                                {
                                    allowedToAdd &&
                                    <>
                                        {addMode === false && (<SupplierRowNew onAdd={handleAddState} />)}

                                        {addMode === true && (
                                            <SupplierRowAdd
                                                onCancel={handleAddState}
                                                onAdd={handleAddNew}
                                                apiError={addApiError} />)}
                                    </>
                                }

                                {suppliersRetrieved?.map((supplier: SupplierView) =>
                                    <SupplierRow
                                        key={supplier.id?.toString()}
                                        supplier={supplier}
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

export default SupplierTable;