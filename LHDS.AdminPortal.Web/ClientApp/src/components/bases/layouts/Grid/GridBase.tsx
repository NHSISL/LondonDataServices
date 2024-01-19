import React, { FunctionComponent } from "react";
import "./GridStyle.css"

interface GridBaseProps {
    children?: React.ReactNode;
    size?: "Full" | "One-Half" | "One-Third" | "Two-Third" | "One-Quarter";
}

const gridSizeMapping: Record<string, string> = {
    Full: "col-sm-12 col-md-12 col-lg-12",
    "One-Half": "col-sm-12 col-md-6 col-lg-6",
    "One-Third": "col-sm-12 col-md-12 col-lg-4",
    "Two-Third": "col-sm-12 col-md-12 col-lg-8",
    "One-Quarter": "col-sm-12 col-md-12 col-lg-3",
};

const GridBase: FunctionComponent<GridBaseProps> = (props) => {
    const classes = gridSizeMapping[props.size || "Full"];
    return (
        <div className={classes}>
            {props.children}
        </div>
       
    )
}

export default GridBase
