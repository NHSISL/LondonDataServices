import { useEffect, useState } from "react";
import { terminologyArtifactService } from "../../foundations/terminologyArtifactService";
import { TerminologyArtifactView } from "../../../models/views/components/terminologyArtifacts/terminologyArtifactsView";
import { TerminologyArtifact } from "../../../models/terminologyArtifacts/terminologyArtifact";

export const terminologyArtifactViewService = {

    useCreateTerminologyArtifact: () => {
        return terminologyArtifactService.useCreateTerminologyArtifact();
    },

    useGetAllTerminologyArtifacts: (searchTerm?: string) => {

        let query = '?$orderby=name';

        if (searchTerm) {
            query = query + `&$filter=contains(name,'${searchTerm}')`;
        }

        const response = terminologyArtifactService.useGetAllTerminologyArtifacts(query);
        const [mappedTerminologyArtifacts, setMappedTerminologyArtifacts] = useState<Array<TerminologyArtifactView>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const terminologyArtifacts = response.data.map((terminologyArtifact: TerminologyArtifact) =>
                    new TerminologyArtifactView(
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
                        terminologyArtifact.isError,
                        terminologyArtifact.errorMessage,
                        terminologyArtifact.createdBy,
                        terminologyArtifact.createdDate,
                        terminologyArtifact.updatedBy,
                        terminologyArtifact.updatedDate,
                    ));

                setMappedTerminologyArtifacts(terminologyArtifacts);
            }
        }, [response.data]);

        return {
            mappedTerminologyArtifacts, ...response
        }
    },

    useGetTerminologyArtifactById: (id: string) => {

        const query = `?$filter=id eq ${id}`
        const response = terminologyArtifactService.useGetAllTerminologyArtifacts(query);
        const [mappedTerminologyArtifact, setMappedTerminologyArtifact] = useState<TerminologyArtifactView>();

        useEffect(() => {
            if (response.data && response.data[0]) {
                const terminologyArtifact = new TerminologyArtifactView(
                    response.data[0].id,
                    response.data[0].fullUrl,
                    response.data[0].resourceType,
                    response.data[0].version,
                    response.data[0].name,
                    response.data[0].title,
                    response.data[0].status,
                    response.data[0].lastUpdated,
                    response.data[0].isCore,
                    response.data[0].isDownloaded,
                    response.data[0].isError,
                    response.data[0].errorMessage,
                    response.data[0].createdBy,
                    response.data[0].createdDate,
                    response.data[0].updatedBy,
                    response.data[0].updatedDate,
                );

                setMappedTerminologyArtifact(terminologyArtifact);
            }
        }, [response.data]);

        return {
            mappedTerminologyArtifact, ...response
        }
    },

    useUpdateTerminologyArtifact: () => {
        return terminologyArtifactService.useUpdateTerminologyArtifact();
    },

    useRemoveTerminologyArtifact: () => {
        return terminologyArtifactService.useDeleteTerminologyArtifact();
    },
}