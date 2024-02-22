import React, { FunctionComponent } from "react";
import SubscriberAgreementDetailCardView from "./subscriberAgreementDetailCardView";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";

interface SubscriberAgreementDetailCardProps {
    children?: React.ReactNode;
}

const SubscriberAgreementDetailCard: FunctionComponent<SubscriberAgreementDetailCardProps> = (props) => {
    const {
        children
    } = props;

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Subscriber Agreement
                    </CardBaseTitle>
                    <CardBaseContent>
                        <SubscriberAgreementDetailCardView/>
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

export default SubscriberAgreementDetailCard;