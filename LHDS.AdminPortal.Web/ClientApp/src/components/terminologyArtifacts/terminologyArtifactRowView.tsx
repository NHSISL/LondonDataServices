import React, { FunctionComponent } from "react";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { TerminologyArtifact } from "../../models/terminologyArtifacts/terminologyArtifact";
import { Link } from 'react-router-dom';
import ButtonBase from "../bases/buttons/ButtonBase";
import moment from "moment";

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
                {terminologyArtifact.resourceType}
            </TableBaseData>
            <TableBaseData>
                    {terminologyArtifact.fullUrl}
            </TableBaseData>
            <TableBaseData>
                    {terminologyArtifact.name}
            </TableBaseData>
            <TableBaseData>
                {moment(terminologyArtifact.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
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