import React, { FunctionComponent } from "react";
import "./ContentStyle.css"

interface ContentBaseProps {
    children?: React.ReactNode;
    classes?: string;
}

const ContentBase: FunctionComponent<ContentBaseProps> = (props) => {
    return (
        <div id="page-content-wrapper">
            <div className={props.classes + " container-fluid"}>
                <div className="row">
                    <div className="col-lg-12">
                        {props.children}
                    </div>
                </div>
            </div>
        </div>
    )
}

export default ContentBase

