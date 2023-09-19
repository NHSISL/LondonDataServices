import React, { FunctionComponent, useState } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import { useNavigate } from "react-router-dom";
import { SpecificationObjectView } from "../../models/views/components/specificationObjects/specificationObjectView";
import SpecificationObjectDetailCardView from "./specificationObjectDetailCardView";
import SpecificationObjectDetailCardEdit from "./specificationObjectDetailCardEdit";

interface SpecificationObjectDetailCardProps {
    specificationObject: SpecificationObjectView;
    dataSetId: string;
    mode: string;
    onAdd: (specificationObject: SpecificationObjectView) => void;
    onUpdate: (specificationObject: SpecificationObjectView) => void;
    onDelete: (specificationObject: SpecificationObjectView) => void;
    children?: React.ReactNode;
}

const SpecificationObjectDetailCard: FunctionComponent<SpecificationObjectDetailCardProps> = (props) => {
    const {
        specificationObject,
        dataSetId,
        mode,
        onAdd,
        onUpdate,
        onDelete,
        children
    } = props;

    const [displayMode, setDisplayMode] = useState<string>(mode);
    const [apiError, setApiError] = useState<any>({});

    const handleModeChange = (value: string) => {
        setDisplayMode(value);
    };

    const navigate = useNavigate();

    const handleAdd = async (specificationObject: SpecificationObjectView) => {
        try {
            onAdd(specificationObject);
            navigate('/configuration/dataSetSpecification/' + dataSetId + '' + specificationObject.dataSetSpecificationId);
        } catch (error) {
            setDisplayMode('EDIT');
        }
    };

    const handleUpdate = async (specificationObject: SpecificationObjectView) => {
        try {
            onUpdate(specificationObject);
            setDisplayMode('VIEW');
        } catch (error) {
            setApiError(error);
            setDisplayMode('EDIT');
        }
    };

    const handleDelete = (specificationObject: SpecificationObjectView) => {
        onDelete(specificationObject);
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
                        {displayMode === "ADD"
                            ? "New Specification Object"
                            : "Specification Object (" + specificationObject.ourObjectName + ")"}
                    </CardBaseTitle>

                    <CardBaseContent>
                        {(displayMode === "VIEW" || displayMode === "CONFIRMDELETE") && (
                            <SpecificationObjectDetailCardView
                                onModeChange={handleModeChange}
                                specificationObject={specificationObject}
                                onDelete={handleDelete}
                                mode={displayMode}
                            />
                        )}
                        {(displayMode === "EDIT" || displayMode === "ADD") && (
                            <SpecificationObjectDetailCardEdit
                                onModeChange={handleModeChange}
                                onAdd={handleAdd}
                                onUpdate={handleUpdate}
                                onCancel={handleCancel}
                                specificationObject={specificationObject}
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

export default SpecificationObjectDetailCard;