import { Guid } from "guid-typescript";
import { useInfiniteQuery, useQuery } from "react-query";
import IngestionTrackingBroker from "../../brokers/apiBroker.ingestionTrackings";

export const ingestionTrackingService = {
    useGetAllIngestionTrackings: (query: string) => {
        const ingestionTrackingBroker = new IngestionTrackingBroker();

        return useQuery(
            ["IngestionTrackingGetAll", { query: query }],
            () => ingestionTrackingBroker.GetAllIngestionTrackingsAsync(query),
            { staleTime: Infinity });
    },

    useGetAllIngestionTrackingPages: (query: string) => {
        const ingestionTrackingBroker = new IngestionTrackingBroker();

        return useInfiniteQuery(
            ["IngestionTrackingGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return ingestionTrackingBroker.GetIngestionTrackingFirstPagesAsync(query)
                }
                return ingestionTrackingBroker.GetIngestionTrackingsSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useGetIngestionTrackingById: (id: Guid) => {
        const ingestionTrackingBroker = new IngestionTrackingBroker();

        return useQuery(
            ["IngestionTrackingGetById", { id: id }],
            () => ingestionTrackingBroker.GetIngestionTrackingByIdAsync(id),
            { staleTime: Infinity });
    }
}