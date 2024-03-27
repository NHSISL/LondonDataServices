import React, { FunctionComponent } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import OptOutDetailCardView from "./optOutDetailCardView";

interface OptOutDetailCardProps {
    optOuts: OptOutView | undefined;
    children?: React.ReactNode;
    onClearCache: (optOuts: OptOutView) => void;
    onAddNewNHS: (optOuts: OptOutView) => void;
    nhsNumber: string,
    isValidNumber: boolean
}

const OptOutDetailCard: FunctionComponent<OptOutDetailCardProps> = (props) => {
    const {
        optOuts,
        children,
        onClearCache,
        onAddNewNHS,
        nhsNumber,
        isValidNumber
    } = props;

    return (
        <div>
            <OptOutDetailCardView
                optOuts={optOuts}
                onClearCache={onClearCache}
                onAddNewNHS={onAddNewNHS}
                isValidNumber={isValidNumber}
                nhsNumber={nhsNumber} />

            {children !== undefined && (
                <>
                    <br />
                    {children}
                </>
            )}
        </div>
    );
}

export default OptOutDetailCard;