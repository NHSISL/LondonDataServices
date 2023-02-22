import React, { FunctionComponent, ReactElement } from "react";

interface InfiniteScrollLoaderProps {
    children?: React.ReactNode;
    loading: boolean;
    spinner: ReactElement;
    noMorePages: boolean;
    noMorePagesMessage: ReactElement
}

const InfiniteScrollLoader: FunctionComponent<InfiniteScrollLoaderProps> = (props) => {
   
    return (
        <>
            {props.children}
            {props.loading && props.spinner}
            {props.noMorePages && props.noMorePagesMessage}
        </>
    )
}

export default InfiniteScrollLoader
