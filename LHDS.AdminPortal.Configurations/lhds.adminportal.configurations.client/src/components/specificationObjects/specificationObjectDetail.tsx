import React, { FunctionComponent, useEffect, useState } from "react";
import { SpecificationObjectView } from "../../models/views/components/specificationObjects/specificationObjectView";
import { specificationObjectViewService } from "../../services/views/specificationObjects/specificationObjectViewService";
import SpecificationObjectDetailCard from "./specificationObjectDetailCard";
import ObjectColumnTable from "../objectColumns/objectColumnTable";

interface SpecificationObjectDetailProps {
    dataSetSpecificationId?: string;
    specificationObjectId?: string;
    dataSetId: string
    children?: React.ReactNode;
}

const SpecificationObjectDetail: FunctionComponent<SpecificationObjectDetailProps> = (props) => {
    const {
        dataSetSpecificationId,
        specificationObjectId,
        dataSetId,
        children
    } = props;

    let specificationObjectRetrieved: SpecificationObjectView | undefined

    if (specificationObjectId !== "" && specificationObjectId !== undefined) {
        const { mappedSpecificationObject } = specificationObjectViewService.useGetSpecificationObjectById(specificationObjectId);
        specificationObjectRetrieved = mappedSpecificationObject;
    }

    const [specificationObject, setSpecificationObject] = useState<SpecificationObjectView>();
    const [mode, setMode] = useState<string>('VIEW');
    const addSpecificationObject = specificationObjectViewService.useCreateSpecificationObject();

    const handleAdd = (specificationObject: SpecificationObjectView) => {
        specificationObject.dataSetSpecificationId = Guid.parse(dataSetSpecificationId!);
        return addSpecificationObject.mutate(specificationObject);
    }

    const updateSpecificationObject = specificationObjectViewService.useUpdateSpecificationObject();

    const handleUpdate = async (specificationObject: SpecificationObjectView) => {
        return updateSpecificationObject.mutateAsync(specificationObject);
    }

    const handleDelete = async (specificationObject: SpecificationObjectView) => {
        return updateSpecificationObject.mutateAsync(specificationObject);
    }

    useEffect(() => {
        if (specificationObjectId !== "" && specificationObjectId !== undefined) {
            setSpecificationObject(specificationObjectRetrieved);
            setMode('VIEW');
        }
        if (specificationObjectId === "" || specificationObjectId === undefined) {
            setSpecificationObject(new SpecificationObjectView(Guid.create(), Guid.parse(dataSetSpecificationId!)));
            setMode('ADD');
        }
    }, [specificationObjectId, specificationObjectRetrieved, dataSetSpecificationId]);

    return (
        <div>
            {specificationObject !== undefined && (
                <div>
                    <SpecificationObjectDetailCard
                        key={specificationObject.id.toString()}
                        specificationObject={specificationObject}
                        dataSetId={dataSetId}
                        mode={mode}
                        onAdd={handleAdd}
                        onUpdate={handleUpdate}
                        onDelete={handleDelete}>
                        {children}
                    </SpecificationObjectDetailCard>

                    {mode !== "ADD" && (
                        <>
                            <ObjectColumnTable
                                key={specificationObject.id.toString()}
                                specificationObjectId={specificationObject.id.toString()}
                                dataSetSpecificationId={dataSetSpecificationId!}
                            >
                            </ObjectColumnTable>

                        </>
                    )}
                </div>
            )}
        </div>
    );
}

export default SpecificationObjectDetail;