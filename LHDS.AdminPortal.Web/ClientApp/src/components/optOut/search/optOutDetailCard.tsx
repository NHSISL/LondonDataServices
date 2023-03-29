import React, { FunctionComponent } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import CardBase from "../../bases/components/Card/CardBase";
import CardBaseBody from "../../bases/components/Card/CardBase.Body";
import CardBaseContent from "../../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../../bases/components/Card/CardBase.Title";
import OptOutDetailCardView from "./optOutDetailCardView";

interface OptOutDetailCardProps {
    optOuts: Array<OptOutView>;
    children?: React.ReactNode;
    onClearCache: (optOuts: Array<OptOutView>) => void;
}

const OptOutDetailCard: FunctionComponent<OptOutDetailCardProps> = (props) => {
    const {
        optOuts,
        children,
        onClearCache
    } = props;

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                       Patient Opt-Out
                    </CardBaseTitle>
                    <CardBaseContent>
                        <OptOutDetailCardView
                            optOuts={optOuts}
                            onClearCache={onClearCache}
                        />
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