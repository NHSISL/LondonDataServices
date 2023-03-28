import React from 'react';

export enum Features {
    OptOut,
    IngestionTracking,
    IngestionTrackingSearch
}

class FeatureSwitchProps {
    feature!: Features;
    children: React.ReactNode;
}   

export const FeatureSwitch = ({ feature, children }: FeatureSwitchProps) => {
    const [enabledFeatures, setEnabledFeatures] = React.useState<Array<Features>>([Features.OptOut, Features.IngestionTracking, Features.IngestionTrackingSearch]);

    if (enabledFeatures.findIndex(f => f === feature) > -1) {
        return <>{children}</>;
    }

    return <></>;
}
