import React, { FunctionComponent } from "react";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { TerminologyArtifact } from "../../models/terminologyArtifacts/terminologyArtifact";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck, faCircleExclamation, faInfoCircle, faStop, faTimes } from "@fortawesome/free-solid-svg-icons";

type TerminologyArtifactRowProps = {
    terminologyArtifact: TerminologyArtifact;
}

const TerminologyArtifactRowView: FunctionComponent<TerminologyArtifactRowProps> = (props) => {
    const {
        terminologyArtifact
    } = props;

    return (
        <TableBaseRow>
            <TableBaseData classes={terminologyArtifact.isError ? "bg-danger text-white" : ""}>
                <span>{terminologyArtifact.resourceType}</span>
            </TableBaseData>
            <TableBaseData classes={terminologyArtifact.isError ? "bg-danger text-white" : ""}>
                <span>{terminologyArtifact.name}</span>
            </TableBaseData>
            <TableBaseData classes={terminologyArtifact.isError ? "bg-danger text-white text-center" : "text-center"}>
                <span>{terminologyArtifact.version}</span>
            </TableBaseData>
            <TableBaseData classes={terminologyArtifact.isError ? "bg-danger text-white text-center" : "text-center"}>
                <span>{terminologyArtifact.status}</span>
            </TableBaseData>

            <TableBaseData classes={terminologyArtifact.isError ? "bg-danger text-white text-center" : "text-center"}>
                <span>
                    {terminologyArtifact.isError ?
                        <FontAwesomeIcon icon={faCircleExclamation} className="" title={terminologyArtifact.errorMessage} /> :
                        <span></span>}
                </span>
            </TableBaseData>

            <TableBaseData classes={terminologyArtifact.isError ? "bg-danger text-white text-center" : "text-center"}>
                <span>
                   {terminologyArtifact.isCore ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" title="isCore" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" title="notCore" />}
                </span>
            </TableBaseData>

            <TableBaseData classes={terminologyArtifact.isError ? "bg-danger text-white text-center" : "text-center"}>
                <span>
                    {terminologyArtifact.isDownloaded ?
                        <FontAwesomeIcon icon={faCheck} className="text-success" title="downloaded" /> :
                        <FontAwesomeIcon icon={faTimes} className="text-danger" title="not downloaded" />}
                </span>
            </TableBaseData>

            <TableBaseData classes={terminologyArtifact.isError ? "bg-danger text-white text-center" : "text-center"}>
                <Link to={`/terminologyArtifactDetail/${terminologyArtifact.id}`}>
                    {
                        terminologyArtifact.isError
                            ? <ButtonBase onClick={() => { }} remove> Details </ButtonBase>
                            : <ButtonBase onClick={() => { }} add> Details </ButtonBase>
                    }
                </Link>
            </TableBaseData>

        </TableBaseRow>
    );
}

export default TerminologyArtifactRowView;