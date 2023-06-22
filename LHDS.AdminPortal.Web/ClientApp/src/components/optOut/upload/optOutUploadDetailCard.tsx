import React, { FunctionComponent } from "react";
import CardBase from "../../bases/components/Card/CardBase";
import CardBaseBody from "../../bases/components/Card/CardBase.Body";
import CardBaseContent from "../../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../../bases/components/Card/CardBase.Title";
import OptOutUploadDetailCardView from "./optOutUploadDetailCardView";

interface OptOutUploadDetailCardProps {
    children?: React.ReactNode;
    onUpload: (data: string[]) => void;
}

const OptOutUploadDetailCard: FunctionComponent<OptOutUploadDetailCardProps> = (props) => {
    const {
        children,
        onUpload
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