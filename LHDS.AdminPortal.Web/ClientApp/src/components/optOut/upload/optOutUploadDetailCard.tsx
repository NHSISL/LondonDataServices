import React, { FunctionComponent } from "react";
import CardBase from "../../bases/components/Card/CardBase";
import CardBaseBody from "../../bases/components/Card/CardBase.Body";
import CardBaseContent from "../../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../../bases/components/Card/CardBase.Title";
import OptOutUploadDetailCardView from "./optOutUploadDetailCardView";
import { OptOut } from "../../../models/optout/optout";

interface OptOutUploadDetailCardProps {
    children?: React.ReactNode;
    onUpload: (data: OptOut[]) => void;
    onUploadSuccess: boolean;
}

const OptOutUploadDetailCard: FunctionComponent<OptOutUploadDetailCardProps> = (props) => {
    const {
        children,
        onUpload,
        onUploadSuccess
    } = props;

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                       Patient Opt-Out
                    </CardBaseTitle>
                    <CardBaseContent>
                        <OptOutUploadDetailCardView
                            onUpload={onUpload}
                            onUploadSuccess={onUploadSuccess}
                            
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

export default OptOutUploadDetailCard;