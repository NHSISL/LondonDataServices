import React, { FunctionComponent, useState } from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import SupplierDetailCardView from "./ingestionTrackingDetailCardView";

interface IngestionTrackingDetailCardProps {
    ingestionTracking: IngestionTrackingView;
    children?: React.ReactNode;
}

const IngestionTrackingDetailCard: FunctionComponent<IngestionTrackingDetailCardProps> = (props) => {
    const {
        ingestionTracking,
        children
    } = props;


    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        EMIS
                    </CardBaseTitle>
                    <CardBaseContent>
                        <SupplierDetailCardView
                            ingestionTracking={ingestionTracking}
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
export default IngestionTrackingDetailCard;