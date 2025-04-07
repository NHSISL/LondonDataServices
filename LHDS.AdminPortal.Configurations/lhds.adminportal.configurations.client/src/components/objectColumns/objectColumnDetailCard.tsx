import React, { FunctionComponent, useState } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import { useNavigate } from "react-router-dom";
import { ObjectColumnView } from "../../models/views/components/objectColumns/objectColumnView";
import ObjectColumnDetailCardView from "./objectColumnDetailCardView";
import ObjectColumnDetailCardEdit from "./objectColumnDetailCardEdit";
import { ApiError } from "../../types/apiError";

interface ObjectColumnViewDetailCardProps {
    objectColumn: ObjectColumnView;
    specificationObjectId: string;
    dataSetSpecificationId: string;
    mode: string;
    onAdd: (objectColumn: ObjectColumnView) => void;
    onUpdate: (objectColumn: ObjectColumnView) => void;
    onDelete: (objectColumn: ObjectColumnView) => void;
    children?: React.ReactNode;
}

const ObjectColumnViewDetailCard: FunctionComponent<ObjectColumnViewDetailCardProps> = (props) => {
    const {
        objectColumn,
        specificationObjectId,
        dataSetSpecificationId,
        mode,
        onAdd,
        onUpdate,
        onDelete,
        children
    } = props;

    const [displayMode, setDisplayMode] = useState<string>(mode);
    const [apiError, setApiError] = useState<ApiError>({ response: { data: { errors: {} } } });

    const handleModeChange = (value: string) => {
        setDisplayMode(value);
    };

    const navigate = useNavigate();

    const handleAdd = async (objectColumn: ObjectColumnView) => {
        try {
            objectColumn.specificationObjectId = specificationObjectId;
            onAdd(objectColumn);
            navigate('/configuration/SpecificationObject/' + specificationObjectId + '/' + dataSetSpecificationId);
        } catch (error) {
            setDisplayMode('EDIT');
            console.log(error);
        }
    };

    const handleUpdate = async (objectColumn: ObjectColumnView) => {
        try {
            objectColumn.specificationObjectId = specificationObjectId;
            onAdd(objectColumn);
            onUpdate(objectColumn);
            setDisplayMode('VIEW');
        } catch (error) {
            setApiError(error);
            setDisplayMode('EDIT');
        }
    };

    const handleDelete = (objectColumn: ObjectColumnView) => {
        onDelete(objectColumn);
        setDisplayMode('VIEW');
    };

    const handleCancel = () => {
        setApiError({});
    }

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        {displayMode === "ADD" ? "New Object Column" : "Object Column (" + objectColumn.ourColumnName + ")"}
                    </CardBaseTitle>

                    <CardBaseContent>
                        {(displayMode === "VIEW" || displayMode === "CONFIRMDELETE") && (
                            <ObjectColumnDetailCardView
                                onModeChange={handleModeChange}
                                objectColumn={objectColumn}
                                onDelete={handleDelete}
                                mode={displayMode}
                            />
                        )}
                        {(displayMode === "EDIT" || displayMode === "ADD") && (
                            <ObjectColumnDetailCardEdit
                                onModeChange={handleModeChange}
                                onAdd={handleAdd}
                                onUpdate={handleUpdate}
                                onCancel={handleCancel}
                                objectColumn={objectColumn}
                                mode={displayMode}
                                apiError={apiError}
                            />
                        )}
                        {children !== undefined && (
                            <>
                                <br />
                                {children}
                            </>
                        )}
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div >
    );
};

export default ObjectColumnViewDetailCard;