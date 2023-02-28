import React from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from '@fortawesome/free-solid-svg-icons'

export const SpinnerBase = () => {
    return (
        <FontAwesomeIcon icon={faSpinner} spin size="2x" className="loadingSpinner" />
    );
}