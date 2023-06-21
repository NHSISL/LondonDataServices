import React, { FunctionComponent } from "react";
import CardBase from "../../bases/components/Card/CardBase";
import CardBaseBody from "../../bases/components/Card/CardBase.Body";
import CardBaseContent from "../../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../../bases/components/Card/CardBase.Title";
import PdsDetailCardView from "./pdsDetailCardView";

interface PdsDetailCardProps {
    children?: React.ReactNode;
}

const PdsDetailCard: FunctionComponent<PdsDetailCardProps> = (props) => {
    const {
        children
    } = props;

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                       Patient Demographic Search
                    </CardBaseTitle>
                    <CardBaseContent>
                        <PdsDetailCardView/>
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

export default PdsDetailCard;