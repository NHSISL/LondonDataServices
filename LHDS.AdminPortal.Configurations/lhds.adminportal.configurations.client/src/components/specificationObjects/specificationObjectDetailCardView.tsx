import React, { FunctionComponent } from "react";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import { SecuredComponents } from "../links";
import securityPoints from "../../securityMatrix";
import { SpecificationObjectView } from "../../models/views/components/specificationObjects/specificationObjectView";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";

interface SpecificationObjectDetailCardViewProps {
    specificationObject: SpecificationObjectView;
    onDelete: (specificationObject: SpecificationObjectView) => void;
    mode: string;
    onModeChange: (value: string) => void;
}

const SpecificationObjectDetailCardView: FunctionComponent<SpecificationObjectDetailCardViewProps> = (props) => {
    const {
        specificationObject,
        onDelete,
        mode,
        onModeChange
    } = props;

    return (
        <>
            <SummaryListBase>
                <SummaryListBaseRow>
                    <SummaryListBaseKey>Supplier Object Name</SummaryListBaseKey>
                    <SummaryListBaseValue>{specificationObject.supplierObjectName}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Our Object Name</SummaryListBaseKey>
                    <SummaryListBaseValue>{specificationObject.ourObjectName}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Object Description</SummaryListBaseKey>
                    <SummaryListBaseValue>{specificationObject.objectDescription}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Interchange Protocol</SummaryListBaseKey>
                    <SummaryListBaseValue>{specificationObject.interchangeProtocol}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Pushed To Us</SummaryListBaseKey>
                    <SummaryListBaseValue>{specificationObject.isPushedToUs ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Deletion Handling</SummaryListBaseKey>
                    <SummaryListBaseValue>{specificationObject.deletionHandling ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Object Description</SummaryListBaseKey>
                    <SummaryListBaseValue>{specificationObject.objectDescription}</SummaryListBaseValue>
                </SummaryListBaseRow>

                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Submission Header Object</SummaryListBaseKey>
                    <SummaryListBaseValue>{specificationObject.isSubmissionHeaderObject ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>


                <SummaryListBaseRow>
                    <SummaryListBaseKey>Is Transaction Log</SummaryListBaseKey>
                    <SummaryListBaseValue>{specificationObject.isTransactionLog ? <FontAwesomeIcon icon={faCheck} className="text-success" /> : <FontAwesomeIcon icon={faTimes} className="text-danger" />}</SummaryListBaseValue>
                </SummaryListBaseRow>

                
            </SummaryListBase>

            {mode === 'VIEW' &&
                <SecuredComponents allowedRoles={securityPoints.dataSets.edit}>
                    <ButtonBase onClick={() => onModeChange('EDIT')} edit>Edit</ButtonBase>
                </SecuredComponents>
            }
            <>
                {mode === 'VIEW' &&
                    <SecuredComponents allowedRoles={securityPoints.dataSets.delete}>
                        <ButtonBase onClick={() => onModeChange('CONFIRMDELETE')} remove>Delete</ButtonBase>
                    </SecuredComponents>
                }
                {mode === 'CONFIRMDELETE' &&
                    <>
                        <SecuredComponents allowedRoles={securityPoints.dataSets.delete}>
                            <>
                                <ButtonBase onClick={() => onModeChange('VIEW')} cancel>Cancel</ButtonBase>
                                <ButtonBase onClick={() => onDelete(specificationObject)} remove>Yes, Delete</ButtonBase>
                            </>
                        </SecuredComponents>
                    </>
                }
            </>
        </>
    );
}

export default SpecificationObjectDetailCardView;
