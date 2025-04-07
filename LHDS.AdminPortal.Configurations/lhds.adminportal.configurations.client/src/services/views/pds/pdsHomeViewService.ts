import { useEffect, useState } from "react";
import { pdsService } from "../../foundations/pdsService";
import { PdsHomeView } from "../../../models/pds/pdsHomeView";

type PdsHomeViewServiceResponse = {
    mappedPds: PdsHomeView[] | undefined;
    pages: Array<{ data: PdsHomeView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: PdsHomeView[] }> } | undefined;
}

export const pdsHomeViewService = {
    useGetAllPds: (searchTerm?: string): PdsHomeViewServiceResponse => {

        let query = `?$orderby=createdDate`;

        if (searchTerm) {
            query = query + `&$filter=contains(message,'${searchTerm}') or contains(fileName,'${searchTerm}')`;
        }

        const response = pdsService.useGetAllPdsPages(query);
        const [mappedPds, setMappedPds] = useState<Array<PdsHomeView>>();;
        const [pages, setPages] = useState<Array<{ data: PdsHomeView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const pdsArray = response.data.pages.flatMap(x => x.data as PdsHomeView[]);
                setMappedPds(pdsArray);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedPds,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },
};
