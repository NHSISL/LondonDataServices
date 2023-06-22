import React, { FunctionComponent } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import ButtonBase from "../../bases/buttons/ButtonBase";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";


interface OptOutDetailCardViewAddProps {
    optOuts: OptOutView | undefined;
    onBack: () => void;
}

const OptOutDetailCardViewAdd: FunctionComponent<OptOutDetailCardViewAddProps> = (props) => {
    const {
        optOuts,
        onBack 
    } = props;

    const handleAddNewNHS = () => {
        onBack();
    };

    return (
        <div>
            <ButtonBase onClick={onBack} view><FontAwesomeIcon icon={faChevronLeft} size="1x" /> Go Back to Search</ButtonBase>
            <br /> <br />
            <p>Add form here.</p>
            <ButtonBase onClick={() => handleAddNewNHS()} add>&nbsp;Add Nhs Number</ButtonBase>&nbsp;
        </div>
    );
};

export default OptOutDetailCardViewAdd;