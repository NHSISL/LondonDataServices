import React, { FunctionComponent, useState } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import CardBase from "../../bases/components/Card/CardBase";
import CardBaseBody from "../../bases/components/Card/CardBase.Body";
import CardBaseContent from "../../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../../bases/components/Card/CardBase.Title";
import OptOutDetailCardView from "./optOutDetailCardView";
import OptOutDetailCardViewAdd from "../add/optOutDetailCardViewAdd";

interface OptOutDetailCardProps {
    optOuts: OptOutView | undefined;
    children?: React.ReactNode;
    onClearCache: (optOuts: OptOutView) => void;
}

const OptOutDetailCard: FunctionComponent<OptOutDetailCardProps> = (props) => {
    const {
        optOuts,
        children,
        onClearCache
    } = props;

    const [showAddView, setShowAddView] = useState(false);

    const handleAddNewNHS = () => {
        setShowAddView(true);
    };

    const handleBack = () => {
        setShowAddView(false);
    };

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                       Patient Opt-Out
                    </CardBaseTitle>
                    <CardBaseContent>

                        {showAddView ? (
                            <OptOutDetailCardViewAdd
                                optOuts={optOuts}
                                onBack={handleBack} />
                        ) : (

                            <OptOutDetailCardView
                                optOuts={optOuts}
                                onClearCache={onClearCache}
                                onAddNewNHS={handleAddNewNHS}
                            />
                        )}
                        {children !== undefined && (
                            <>
                                <br />
                                {children}
                            </>
                        )}

                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
}

export default OptOutDetailCard;