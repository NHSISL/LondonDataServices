import React, { FunctionComponent } from "react";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import SupplierDetailCardView from "./ingestionTrackingDetailCardView";
import IngestionTrackingAuditTable from "../ingestionTrackingAudit/ingestionTrackingAuditTable";

interface IngestionTrackingDetailCardProps {
    ingestionTracking: IngestionTrackingView;
    children?: React.ReactNode;
    onReDecrypt: (ingestionTracking: IngestionTrackingView) => void;
}

const IngestionTrackingDetailCard: FunctionComponent<IngestionTrackingDetailCardProps> = (props) => {
    const {
        ingestionTracking,
        children,
        onReDecrypt
    } = props;

    const handleReDecrypt = async (ingestionTracking: IngestionTrackingView) => {
        await onReDecrypt(ingestionTracking);
    }

    return (
        <>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        EMIS
                    </CardBaseTitle>
                    <CardBaseContent>
                        <SupplierDetailCardView
                            ingestionTracking={ingestionTracking}
                            onReDecrypt={handleReDecrypt} />

                        {children !== undefined && (
                            <>
                                {children}
                            </>
                        )}
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>

            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Audit
                    </CardBaseTitle>
                    <CardBaseContent>
                        <IngestionTrackingAuditTable ingestionTrackingId={ingestionTracking.id}></IngestionTrackingAuditTable>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </>
    );
}
export default IngestionTrackingDetailCard;