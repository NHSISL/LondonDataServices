import React, { FunctionComponent, useState } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import { useNavigate } from "react-router-dom";
import { DataSetSpecificationView } from "../../models/views/components/dataSetSpecifications/dataSetSpecificationView";
import DataSetSpecificationDetailCardView from "./dataSetSpecificationDetailCardView";
import DataSetSpecificationDetailCardEdit from "./dataSetSpecificationDetailCardEdit";
import { ApiError } from "../../types/apiError";

interface DataSetSpecificationDetailCardProps {
    dataSetSpecification: DataSetSpecificationView;
    dataSetId?: string;
    mode: string;
    onAdd: (dataSetSpecification: DataSetSpecificationView) => void;
    onUpdate: (dataSetSpecification: DataSetSpecificationView) => void;
    onDelete: (dataSetSpecification: DataSetSpecificationView) => void;
    children?: React.ReactNode;
}

const DataSetSpecificationDetailCard: FunctionComponent<DataSetSpecificationDetailCardProps> = (props) => {
    const {
        dataSetSpecification,
        dataSetId,
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

    const handleAdd = async (dataSetSpecification: DataSetSpecificationView) => {
        try {
            dataSetSpecification.dataSetId = dataSetId!;
            onAdd(dataSetSpecification);
            navigate('/configuration/dataSet/' + dataSetId);
        } catch (error) {
            setDisplayMode('EDIT');
            console.log(error);
        }
    };

    const handleUpdate = async (dataSetSpecification: DataSetSpecificationView) => {
        try {
            dataSetSpecification.dataSetId = dataSetId!;
            onUpdate(dataSetSpecification);
            setDisplayMode('VIEW');
        } catch (error) {
            setApiError(error);
            setDisplayMode('EDIT');
        }
    };

    const handleDelete = (dataSetSpecification: DataSetSpecificationView) => {
        onDelete(dataSetSpecification);
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
                            ? "New DataSet Specification"
                            : "DataSet Specification (" + dataSetSpecification.ourSpecificationVersion + ")"}
                    </CardBaseTitle>

                    <CardBaseContent>
                        {(displayMode === "VIEW" || displayMode === "CONFIRMDELETE") && (
                            <DataSetSpecificationDetailCardView
                                onModeChange={handleModeChange}
                                dataSetSpecification={dataSetSpecification}
                                onDelete={handleDelete}
                                mode={displayMode}
                            />
                        )}
                        {(displayMode === "EDIT" || displayMode === "ADD") && (
                            <DataSetSpecificationDetailCardEdit
                                onModeChange={handleModeChange}
                                onAdd={handleAdd}
                                onUpdate={handleUpdate}
                                onCancel={handleCancel}
                                dataSetSpecification={dataSetSpecification}
                                dataSetId={dataSetId!.toString()}
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

export default DataSetSpecificationDetailCard;