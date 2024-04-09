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
    onDownload: (ingestionTracking: IngestionTrackingView) => void;
    onReLand: (ingestionTracking: IngestionTrackingView) => void;
    onRefresh: (ingestionTracking: IngestionTrackingView) => void;
}

const IngestionTrackingDetailCard: FunctionComponent<IngestionTrackingDetailCardProps> = (props) => {
    const {
        ingestionTracking,
        children,
        onDownload,
        onReLand,
        onRefresh
    } = props;


    const handleDownload = async (ingestionTracking: IngestionTrackingView) => {
        await onDownload(ingestionTracking);
    }

    const handleReland = async (ingestionTracking: IngestionTrackingView) => {
        await onReLand(ingestionTracking);
    }

    const handlRefresh = async (ingestionTracking: IngestionTrackingView) => {
        await onRefresh(ingestionTracking);
    }

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
                            onDownload={handleDownload}
                            onReLand={handleReland}
                            onRefresh={handlRefresh} />

                        {children !== undefined && (
                            <>
                                <br />
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
        </div>
    );
}
export default IngestionTrackingDetailCard;