import { useQuery } from "react-query";
import FeatureBroker from "../../brokers/apiBroker.features";
import { Feature } from "../../models/features/feature";

export const featureService = {
    useGetAllFeatures: () => {
        const featureBroker = new FeatureBroker();

        return useQuery <Feature[]>(
            ["FeaturesGetAll"],
            () => featureBroker.GetAllFeatureAsync(),
            { staleTime: Infinity });
    }
}