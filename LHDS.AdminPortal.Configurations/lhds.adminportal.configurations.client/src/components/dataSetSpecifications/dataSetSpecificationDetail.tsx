import React, { FunctionComponent, useEffect, useState } from "react";
import { DataSetSpecificationView } from "../../models/views/components/dataSetSpecifications/dataSetSpecificationView";
import { dataSetSpecificationViewService } from "../../services/views/dataSetSpecification/dataSetSpecificationViewService";
import DataSetSpecificationDetailCard from "./dataSetSpecificationDetailCard";
import SpecificationObjectTable from "../specificationObjects/specificationObjectTable";

interface DataSetSpecificationDetailProps {
    dataSetId?: string;
    dataSetSpecificationId?: string;
    children?: React.ReactNode;
}

const DataSetSpecificationDetail: FunctionComponent<DataSetSpecificationDetailProps> = (props) => {
    const {
        dataSetId,
        dataSetSpecificationId,
        children
    } = props;

    let dataSetSpecificationRetrieved: DataSetSpecificationView | undefined

    if (dataSetSpecificationId !== "" && dataSetSpecificationId !== undefined) {
        const { mappedDataSetSpecification } = dataSetSpecificationViewService.useGetDataSetSpecificationById(dataSetSpecificationId);
        dataSetSpecificationRetrieved = mappedDataSetSpecification;
    }

    const [dataSetSpecification, setDataSetSpecification] = useState<DataSetSpecificationView>();
    const [mode, setMode] = useState<string>('VIEW');
    const addDataSetSpecification = dataSetSpecificationViewService.useCreateDataSetSpecification();

    const handleAdd = (dataSetSpecification: DataSetSpecificationView) => {
        return addDataSetSpecification.mutate(dataSetSpecification);
    }

    const updateDataSetSpecification = dataSetSpecificationViewService.useUpdateDataSetSpecification();

    const handleUpdate = async (dataSetSpecification: DataSetSpecificationView) => {
        return updateDataSetSpecification.mutateAsync(dataSetSpecification);
    }

    const handleDelete = async (dataSetSpecification: DataSetSpecificationView) => {
        dataSetSpecification.isActive = false;
        return updateDataSetSpecification.mutateAsync(dataSetSpecification);
    }

    useEffect(() => {
        if (dataSetSpecificationId !== "" && dataSetSpecificationId !== undefined) {
            setDataSetSpecification(dataSetSpecificationRetrieved);
            setMode('VIEW');
        }
        if (dataSetSpecificationId === "" || dataSetSpecificationId === undefined) {
            setDataSetSpecification(new DataSetSpecificationView(
                Guid.create(),
                Guid.parse(dataSetId!)
            ));
            setMode('ADD');
        }
    }, [dataSetSpecificationId, dataSetSpecificationRetrieved, dataSetId]);

    return (
        <div>
            {dataSetSpecification !== undefined && (
                <div>
                    <DataSetSpecificationDetailCard
                        key={dataSetSpecification.id.toString()}
                        dataSetSpecification={dataSetSpecification}
                        dataSetId={dataSetId!.toString()}
                        mode={mode}
                        onAdd={handleAdd}
                        onUpdate={handleUpdate}
                        onDelete={handleDelete}
                    >
                        {children}
                    </DataSetSpecificationDetailCard>

                    {mode !== "ADD" && (
                        <>
                            <SpecificationObjectTable
                                key={dataSetSpecification.id.toString()}
                                dataSetSpecificationId={dataSetSpecification.id.toString()}
                                dataSetId={dataSetId!.toString()}>
                            </SpecificationObjectTable>
                        </>
                    )}
                </div>
            )}
        </div>
    );
}

export default DataSetSpecificationDetail;