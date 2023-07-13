import { faUpload } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { ChangeEvent, FunctionComponent, useState } from "react";
import ButtonBase from "../../bases/buttons/ButtonBase";

interface PdsUploadDetailCardViewProps {
    onUpload: (data: string[]) => void;
}

const PdsUploadDetailCardView: FunctionComponent<PdsUploadDetailCardViewProps> = (props) => {
    const { onUpload } = props;


    return (
        <>
            <div>
                TODO:
            </div>
        </>
    );
};

export default PdsUploadDetailCardView;
