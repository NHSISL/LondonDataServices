import React, { FunctionComponent, useEffect, useState } from "react";
import { DataSetView } from "../../models/views/components/dataSets/dataSetView";
import { dataSetViewService } from "../../services/views/dataSets/dataSetViewService";
import DataSetDetailCard from "./dataSetDetailCard";
import DataSetSpecificationTable from "../dataSetSpecifications/dataSetSpecificationTable";

interface DataSetDetailProps {
    dataSetId?: string;
    children?: React.ReactNode;
}

const DataSetDetail: FunctionComponent<DataSetDetailProps> = (props) => {
    const {
        dataSetId,
        children
    } = props;

    let dataSetRetrieved: DataSetView | undefined

    if (dataSetId !== "" && dataSetId !== undefined) {
        const { mappedDataSet } = dataSetViewService.useGetDataSetById(dataSetId ?? "");
        dataSetRetrieved = mappedDataSet;
    }

    const [dataSet, setDataSet] = useState<DataSetView>();
    const [mode, setMode] = useState<string>('VIEW');
    const addDataSet = dataSetViewService.useCreateDataSet();

    const handleAdd = (dataSet: DataSetView) => {
        return addDataSet.mutate(dataSet);
    }

    const updateDataSet = dataSetViewService.useUpdateDataSet();

    const handleUpdate = async (dataSet: DataSetView) => {
        return updateDataSet.mutateAsync(dataSet);
    }

    const handleDelete = async (dataSet: DataSetView) => {
        dataSet.isActive = false;
        return updateDataSet.mutateAsync(dataSet);
    }

    useEffect(() => {
        if (dataSetId !== "" && dataSetRetrieved !== undefined) {
            setDataSet(dataSetRetrieved);
            setMode('VIEW');
        }
        if (dataSetId === "" || dataSetId === undefined) {
            setDataSet(new DataSetView(crypto.randomUUID(), "", "", "", "", "", false, "", false, "", true, new Date(), new Date()))
            setMode('ADD');
        }
    }, [dataSetId, dataSetRetrieved]);

    return (
        <div>
            {dataSet !== undefined && (
                <div>
                    <DataSetDetailCard
                        key={dataSet.id.toString()}
                        dataSet={dataSet}
                        mode={mode}
                        onAdd={handleAdd}
                        onUpdate={handleUpdate}
                        onDelete={handleDelete}
                    >
                        {children}
                    </DataSetDetailCard>

                    {mode !== "ADD" && (
                        <>
                            <DataSetSpecificationTable
                                key={dataSet.id.toString()}
                                dataSetId={dataSet.id.toString()}                            >
                            </DataSetSpecificationTable>
                        </>
                    )}
                </div>
            )}
        </div>
    );
}

export default DataSetDetail;