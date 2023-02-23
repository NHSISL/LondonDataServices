import React from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import SupplierDetailCardView from "./ingestionTrackingDetailCardView";

const IngestionTrackingDetailCard = () => {

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        EMIS
                    </CardBaseTitle>
                    <CardBaseContent>
                        <SupplierDetailCardView />
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
}
export default IngestionTrackingDetailCard;