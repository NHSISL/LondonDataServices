import { useQuery, useInfiniteQuery } from "@tanstack/react-query";
import IngestionTrackingBroker from "../../brokers/apiBroker.ingestionTrackings";

export const ingestionTrackingService = {
    useGetAllIngestionTrackings: (query: string) => {
        const ingestionTrackingBroker = new IngestionTrackingBroker();

        return useQuery({
            queryKey: ["IngestionTrackingGetAll", { query: query }],
            queryFn: () => ingestionTrackingBroker.GetAllIngestionTrackingsAsync(query),
            staleTime: Infinity
        });
    },

    useGetAllIngestionTrackingPages: (query: string) => {
        const ingestionTrackingBroker = new IngestionTrackingBroker();

        return useInfiniteQuery({
            queryKey: ["IngestionTrackingGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return ingestionTrackingBroker.GetIngestionTrackingFirstPagesAsync(query)
                }
                return ingestionTrackingBroker.GetIngestionTrackingsSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useGetIngestionTrackingById: (id: string) => {
        const ingestionTrackingBroker = new IngestionTrackingBroker();

        return useQuery({
            queryKey: ["IngestionTrackingGetById", { id: id }],
            queryFn: () => ingestionTrackingBroker.GetIngestionTrackingByIdAsync(id),
            staleTime: Infinity
        });
    }
}