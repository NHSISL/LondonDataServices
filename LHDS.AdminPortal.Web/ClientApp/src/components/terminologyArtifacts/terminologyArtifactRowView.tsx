import React, { FunctionComponent } from "react";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { TerminologyArtifact } from "../../models/terminologyArtifacts/terminologyArtifact";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import moment from "moment";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";

type TerminologyArtifactRowProps = {
    terminologyArtifact: TerminologyArtifact;
}

const TerminologyArtifactRowView: FunctionComponent<TerminologyArtifactRowProps> = (props) => {
    const {
        terminologyArtifact
    } = props;

    return (
        <TableBaseRow>
            <TableBaseData>
                <span style={{ fontSize: "12px" }}>{terminologyArtifact.resourceType}</span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: "12px" }}>{terminologyArtifact.fullUrl}</span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: "12px" }}>{terminologyArtifact.name}</span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: "12px" }}>
                   {terminologyArtifact.isCore ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" title="isCore" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" title="notCore" />}
                </span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: "12px" }}>
                    {terminologyArtifact.isDownloaded ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" title="downloaded" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" title="not downloaded" />}
                </span>
            </TableBaseData>
            <TableBaseData>
                <span style={{ fontSize: "12px" }}>
                    {moment(terminologyArtifact.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                </span>
            </TableBaseData>
            <TableBaseData>
                <Link to={`/terminologyArtifactDetail/${terminologyArtifact.id}`}>
                    <ButtonBase onClick={() => { }} add>
                        Details
                    </ButtonBase>
                </Link>
            </TableBaseData>
        </TableBaseRow>
    );
}

export default TerminologyArtifactRowView;