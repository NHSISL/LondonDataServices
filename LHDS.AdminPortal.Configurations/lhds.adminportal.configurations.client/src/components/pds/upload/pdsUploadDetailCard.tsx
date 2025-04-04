import React, { FunctionComponent } from "react";
import CardBase from "../../bases/components/Card/CardBase";
import CardBaseBody from "../../bases/components/Card/CardBase.Body";
import CardBaseContent from "../../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../../bases/components/Card/CardBase.Title";
import PdsUploadDetailCardView from "./pdsUploadDetailCardView";

interface PdsUploadDetailCardProps {
    children?: React.ReactNode;
    onUpload: (data: string[]) => void;
}

const PdsUploadDetailCard: FunctionComponent<PdsUploadDetailCardProps> = (props) => {
    const {
        children,
        onUpload
    } = props;

    return (
        <div>
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                       Patient Demographic Search Upload
                    </CardBaseTitle>
                    <CardBaseContent>
                        <PdsUploadDetailCardView
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

export default PdsUploadDetailCard;