import React, { FunctionComponent } from "react";
import PdsRowView from "./pdsRowView";
import { Pds } from "../../../models/pds/pds";

type PdsRowProps = {
    pds: Pds;
};

const PdsRow: FunctionComponent<PdsRowProps> = (props) => {
    const { pds } = props;
    return (
        <PdsRowView
            key={pds.id.toString()}
            pds={pds} />
    );
};

export default PdsRow;