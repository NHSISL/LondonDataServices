import React, { FunctionComponent, useState } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import { useNavigate } from "react-router-dom";
import { DataSetView } from "../../models/views/components/dataSets/dataSetView";
import DataSetDetailCardView from "./dataSetDetailCardView";
import DataSetDetailCardEdit from "./dataSetDetailCardEdit";

interface DataSetDetailCardProps {
    dataSet: DataSetView;
    mode: string;
    onAdd: (dataSet: DataSetView) => void;
    onUpdate: (dataSet: DataSetView) => void;
    onDelete: (dataSet: DataSetView) => void;
    children?: React.ReactNode;
}

const DataSetDetailCard: FunctionComponent<DataSetDetailCardProps> = (props) => {
    const {
        dataSet,
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

    const handleAdd = async (dataSet: DataSetView) => {
        try {
            await onAdd(dataSet);
            navigate('/configuration/dataSets');
        } catch (error) {
            setDisplayMode('EDIT');
        }
    };

    const handleUpdate = async (dataSet: DataSetView) => {
        try {
            await onUpdate(dataSet);
            setDisplayMode('VIEW');
        } catch (error) {
            setApiError(error);
            setDisplayMode('EDIT');
        }
    };

    const handleDelete = (dataSet: DataSetView) => {
        onDelete(dataSet);
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
                        {displayMode === "ADD" ? "New DataSet" : dataSet.dataSetName}
                    </CardBaseTitle>

                    <CardBaseContent>
                        {(displayMode === "VIEW" || displayMode === "CONFIRMDELETE") && (
                            <DataSetDetailCardView
                                onModeChange={handleModeChange}
                                dataSet={dataSet}
                                onDelete={handleDelete}
                                mode={displayMode}
                            />
                        )}
                        {(displayMode === "EDIT" || displayMode === "ADD") && (
                            <DataSetDetailCardEdit
                                onModeChange={handleModeChange}
                                onAdd={handleAdd}
                                onUpdate={handleUpdate}
                                onCancel={handleCancel}
                                dataSet={dataSet}
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

export default DataSetDetailCard;