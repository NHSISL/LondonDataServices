import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
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

interface TerminologyArtifactDetailCardViewProps {
    terminologyArtifact: TerminologyArtifactView;
    onRefresh: (terminologyArtifact: TerminologyArtifactView) => void;
}

const TerminologyArtifactDetailCardView: FunctionComponent<TerminologyArtifactDetailCardViewProps> = (props) => {
    const {
        terminologyArtifact,
    } = props;

    return (
        <>
            <div className="row">
                <GridBase size="Two-Third">
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
                                            {moment(terminologyArtifact.lastUpdated?.toString()).format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>

                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Is Core</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.isCore ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>

                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Is Downloaded</SummaryListBaseKey>
                                        <SummaryListBaseValue>{terminologyArtifact.isDownloaded ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>


                                     <SummaryListBaseRow>
                                        <SummaryListBaseKey>Created Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(terminologyArtifact.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Updated Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(terminologyArtifact.updatedDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                </SummaryListBase>

                            </CardBaseContent>
                        </CardBaseBody>
                    </CardBase>
                </GridBase>
            </div>
        </>
    );
}

export default TerminologyArtifactDetailCardView;