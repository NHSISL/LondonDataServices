import { useEffect, useState } from "react";
import { TerminologyArtifactHomeView } from "../../../models/terminologyArtifacts/terminologyArtifactHomeView";
import { TerminologyArtifact } from "../../../models/terminologyArtifacts/terminologyArtifact";
import { terminologyArtifactService } from "../../foundations/terminologyArtifactService";

type TerminologyArtifactHomeViewServiceResponse = {
    mappedTerminologyArtifacts: TerminologyArtifactHomeView[] | undefined;
    pages: any;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: any;
    refetch: () => void
}

export const TerminologyArtifactHomeViewService = {
    useGetAllTerminologyArtifacts: (searchTerm?: string): TerminologyArtifactHomeViewServiceResponse => {
        try {
            let query = `?$orderby=createdDate`;

            if (searchTerm) {
                query = query + `&$filter=contains(name,'${searchTerm}')`;
            }

            const response = terminologyArtifactService.useGetAllTerminologyArtifactsPages(query);

            const [mappedTerminologyArtifacts, setMappedTerminologyArtifacts] =
                useState<Array<TerminologyArtifactHomeView>>();

            const [pages, setPages] = useState<any>([]);

            useEffect(() => {
                if (response.data && response.data.pages) {
                    const terminologyArtifactArray: Array<TerminologyArtifactHomeView> = [];

                    response.data.pages.forEach((page: any) => {
                        page.data.forEach((terminologyArtifact: TerminologyArtifact) => {
                            terminologyArtifactArray.push(new TerminologyArtifactHomeView(
                                terminologyArtifact.id,
                                terminologyArtifact.fullUrl,
                                terminologyArtifact.resourceType,
                                terminologyArtifact.version,
                                terminologyArtifact.name,
                                terminologyArtifact.title,
                                terminologyArtifact.status,
                                terminologyArtifact.lastUpdated,
                                terminologyArtifact.isCore,
                                terminologyArtifact.isDownloaded,
                                terminologyArtifact.createdBy,
                                terminologyArtifact.createdDate,
                                terminologyArtifact.updatedBy,
                                terminologyArtifact.updatedDate,
                            ));
                        });
                    });

                    setMappedTerminologyArtifacts(terminologyArtifactArray);
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
        } catch (err) {
            throw err;
        }
    },
};
