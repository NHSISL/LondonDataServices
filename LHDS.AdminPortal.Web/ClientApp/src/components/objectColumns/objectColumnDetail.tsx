import React, { FunctionComponent, useEffect, useState } from "react";
import { Guid } from 'guid-typescript';
import { ObjectColumnView } from "../../models/views/components/objectColumns/objectColumnView";
import { objectColumnViewService } from "../../services/views/objectColumns/objectColumnViewService";
import ObjectColumnDetailCard from "./objectColumnDetailCard";

interface ObjectColumnDetailProps {
    specificationObjectId?: string;
    objectColumnId?: string;
    children?: React.ReactNode;
}

const ObjectColumnDetail: FunctionComponent<ObjectColumnDetailProps> = (props) => {
    const {
        specificationObjectId,
        objectColumnId,
        children
    } = props;

    let objectColumnRetrieved: ObjectColumnView | undefined

    if (objectColumnId !== "" && objectColumnId !== undefined) {
        let { mappedObjectColumn } = objectColumnViewService.useGetObjectColumnById(Guid.parse(objectColumnId));
        objectColumnRetrieved = mappedObjectColumn;
    }

    const [objectColumn, setObjectColumn] = useState<ObjectColumnView>();
    const [mode, setMode] = useState<string>('VIEW');
    const addObjectColumn = objectColumnViewService.useCreateObjectColumn();

    const handleAdd = (objectColumn: ObjectColumnView) => {
        return addObjectColumn.mutate(objectColumn);
    }

    const updateObjectColumn = objectColumnViewService.useUpdateObjectColumn();

    const handleUpdate = async (objectColumn: ObjectColumnView) => {
        return updateObjectColumn.mutateAsync(objectColumn);
    }

    const handleDelete = async (objectColumn: ObjectColumnView) => {
        return updateObjectColumn.mutateAsync(objectColumn);
    }

    useEffect(() => {
        if (objectColumnId !== "" && objectColumnId !== undefined) {
            setObjectColumn(objectColumnRetrieved);
            setMode('VIEW');
        }
        if (objectColumnId === "" || objectColumnId === undefined) {
            setObjectColumn(new ObjectColumnView(Guid.create(), Guid.parse(specificationObjectId!)));
            setMode('ADD');
        }
    }, [objectColumnId, objectColumnRetrieved, specificationObjectId]);

    return (
        <div>
            {objectColumn !== undefined && (
                <div>
                    <ObjectColumnDetailCard
                        key={objectColumn.id.toString()}
                        objectColumn={objectColumn}
                        mode={mode}
                        onAdd={handleAdd}
                        onUpdate={handleUpdate}
                        onDelete={handleDelete}>
                        {children}
                    </ObjectColumnDetailCard>

                    {mode !== "ADD" && (
                        <></>
                    )}
                </div>
            )}
        </div>
    );
}

export default ObjectColumnDetail;