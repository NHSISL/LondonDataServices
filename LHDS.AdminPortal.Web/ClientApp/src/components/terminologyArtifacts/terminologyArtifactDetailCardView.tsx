import { faCheck, faCircleExclamation, faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { FunctionComponent } from "react";
import moment from "moment";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import GridBase from "../bases/layouts/Grid/GridBase";
import { TerminologyArtifactView } from "../../models/views/components/terminologyArtifacts/terminologyArtifactsView";
import ButtonBase from "../bases/buttons/ButtonBase";
import SummaryListBaseAction from "../bases/components/SummaryList/SummaryListBase.Action";

interface TerminologyArtifactDetailCardViewProps {
    terminologyArtifact: TerminologyArtifactView;
    onRefresh: (terminologyArtifact: TerminologyArtifactView) => void;
    onUpdate: (terminologyArtifact: TerminologyArtifactView) => void;
}

const TerminologyArtifactDetailCardView: FunctionComponent<TerminologyArtifactDetailCardViewProps> = (props) => {
    const {
        terminologyArtifact,
        onUpdate
    } = props;


    const handleIsCoreUpdate = () => {
        terminologyArtifact.isCore = true;
        onUpdate(terminologyArtifact);
    }

    const handleIsNotCoreUpdate = () => {
        terminologyArtifact.isCore = false;
        onUpdate(terminologyArtifact);
    }

    const handleIsNotDownloadedUpdate = () => {
        terminologyArtifact.isDownloaded = false;
        onUpdate(terminologyArtifact);
    }

    const handleIsForUserUpdate = () => {
        terminologyArtifact.isForUser = true;
        onUpdate(terminologyArtifact);
    }

    const handleIsNotForUserUpdate = () => {
        terminologyArtifact.isForUser = false;
        onUpdate(terminologyArtifact);
    }

    const handleIsNotDownloadedForUserUpdate = () => {
        terminologyArtifact.isDownloadedForUser = false;
        onUpdate(terminologyArtifact);
    }

    return (
        <>
            <div className="row">
                <GridBase>
                    <CardBase>
                        <CardBaseBody>
                            <CardBaseTitle>
                                Details
                            </CardBaseTitle>
                            <CardBaseContent>
                                <SummaryListBase>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Full Url</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.fullUrl}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Resource Type</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.resourceType}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Artifact Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.name}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Version</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.version}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Title</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.title}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Status</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.status}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Last Updated</SummaryListBaseKey>
                                        <SummaryListBaseValue>

                                            {moment(terminologyArtifact.lastUpdated?.toString())
                                                .format("Do-MMM-yyyy HH:mm")}

                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>

                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Is Core</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.isCore ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                        <SummaryListBaseAction>
                                            <span style={{ float: "right" }}>
                                                {terminologyArtifact.isCore ?
                                                    <ButtonBase onClick={handleIsNotCoreUpdate} remove>
                                                        Mark Not Core
                                                    </ButtonBase> :
                                                    <span>
                                                        <ButtonBase onClick={handleIsCoreUpdate} add>
                                                            Mark Core
                                                        </ButtonBase>
                                                    </span>}
                                            </span>
                                        </SummaryListBaseAction>
                                        
                                    </SummaryListBaseRow>

                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Is Downloaded</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.isDownloaded ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                        <SummaryListBaseAction>
                                            <span style={{ float: "right" }}>
                                                {terminologyArtifact.isDownloaded ?
                                                    <ButtonBase onClick={handleIsNotDownloadedUpdate} remove>
                                                        Mark Not Downloaded
                                                    </ButtonBase> : "" }
                                            </span>
                                        </SummaryListBaseAction>
                                    </SummaryListBaseRow>

                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Is For User</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.isForUser ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                        <SummaryListBaseAction>
                                            <span style={{ float: "right" }}>
                                                {terminologyArtifact.isForUser ?
                                                    <ButtonBase onClick={handleIsNotForUserUpdate} remove>
                                                        Mark Not For User
                                                    </ButtonBase> :
                                                    <span>
                                                        <ButtonBase onClick={handleIsForUserUpdate} add>
                                                            Mark For User
                                                        </ButtonBase>
                                                    </span>}
                                            </span>
                                        </SummaryListBaseAction>

                                    </SummaryListBaseRow>

                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Is Downloaded For User</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.isDownloadedForUser ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                        <SummaryListBaseAction>
                                            <span style={{ float: "right" }}>
                                                {terminologyArtifact.isDownloadedForUser ?
                                                    <ButtonBase onClick={handleIsNotDownloadedForUserUpdate} remove>
                                                        Mark Not Downloaded For User
                                                    </ButtonBase> : ""}
                                            </span>
                                        </SummaryListBaseAction>
                                    </SummaryListBaseRow>


                                     <SummaryListBaseRow>
                                        <SummaryListBaseKey>Created Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(terminologyArtifact.createdDate?.toString())
                                                .format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>

                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Updated Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(terminologyArtifact.updatedDate?.toString())
                                                .format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>

                                    <SummaryListBaseRow classes={terminologyArtifact.isError ? "text-danger" : ""}>
                                        <SummaryListBaseKey>Is Error</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.isError ?
                                            <FontAwesomeIcon icon={faCircleExclamation} className="text-danger" /> :
                                            <span></span>}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                </SummaryListBase>

                                <SummaryListBaseRow classes={terminologyArtifact.isError ? "text-danger" : ""}>
                                    <SummaryListBaseKey>Error Message</SummaryListBaseKey>
                                    <SummaryListBaseValue>{terminologyArtifact.errorMessage}</SummaryListBaseValue>
                                </SummaryListBaseRow>
                            </CardBaseContent>
                        </CardBaseBody>
                    </CardBase>
                </GridBase>
            </div>
        </>
    );
}

export default TerminologyArtifactDetailCardView;