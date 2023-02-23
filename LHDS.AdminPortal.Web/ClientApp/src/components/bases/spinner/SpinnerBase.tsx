import React from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faStethoscope } from '@fortawesome/free-solid-svg-icons'

export const SpinnerBase = () => {
    return (
        <FontAwesomeIcon icon={faStethoscope} spin size="2x" className="loadingSpinner"/>
    );
}