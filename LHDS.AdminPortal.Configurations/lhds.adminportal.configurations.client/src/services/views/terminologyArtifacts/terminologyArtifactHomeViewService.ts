import { useEffect, useState } from "react";
import { TerminologyArtifactHomeView } from "../../../models/terminologyArtifacts/terminologyArtifactHomeView";
import { terminologyArtifactService } from "../../foundations/terminologyArtifactService";

type TerminologyArtifactHomeViewServiceResponse = {
    mappedTerminologyArtifacts: TerminologyArtifactHomeView[] | undefined;
    pages: Array<{ data: TerminologyArtifactHomeView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: TerminologyArtifactHomeView[] }> } | undefined;
    refetch: () => void
}

export const TerminologyArtifactHomeViewService = {
    useGetAllTerminologyArtifacts: (searchTerm?: string, resourceType?: string): TerminologyArtifactHomeViewServiceResponse => {

        let query = `?$orderby=isError desc, createdDate desc, version desc`;

        if (searchTerm) {
            query += `&$filter=contains(name,'${searchTerm}')`;
        }

        if (resourceType && resourceType !== "all") {
            if (query.includes('&$filter=')) {
                query += ` and ResourceType eq '${resourceType}'`;
            } else {
                query += `&$filter=ResourceType eq '${resourceType}'`;
            }
        }

        const response = terminologyArtifactService.useGetAllTerminologyArtifactsPages(query);
        const [mappedTerminologyArtifacts, setmappedTerminologyArtifacts] = useState<Array<TerminologyArtifactHomeView>>();
        const [pages, setPages] = useState<Array<{ data: TerminologyArtifactHomeView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const terminologyArtifacts = response.data.pages.flatMap(x => x.data as TerminologyArtifactHomeView[]);
                setmappedTerminologyArtifacts(terminologyArtifacts);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedTerminologyArtifacts,
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
