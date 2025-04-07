import { useEffect, useState } from "react";
import { ObjectColumnView } from "../../../models/views/components/objectColumns/objectColumnView";
import { objectColumnService } from "../../foundations/objectColumnService";

type ObjectColumnViewServiceResponse = {
    mappedObjectColumns: ObjectColumnView[] | undefined;
    pages: Array<{ data: ObjectColumnView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: ObjectColumnView[] }> } | undefined;
    refetch: () => void
}

export const objectColumnViewService = {
    useCreateObjectColumn: () => {
        return objectColumnService.useCreateObjectColumn();
    },

    useGetAllObjectColumns: (specificationObjectId: string, searchTerm?: string): ObjectColumnViewServiceResponse => {
        let query = `?$filter=specificationObjectId eq ${specificationObjectId}&$expand=SpecificationObject&$orderby=createdDate desc`;

        if (searchTerm) {
            query = query + `&$filter=contains(ourObjectName,'${searchTerm}')`;
        }

        const response = objectColumnService.useRetrieveAllObjectColumnPages(query);
        const [mappedObjectColumns, setMappedObjectColumns] = useState<Array<ObjectColumnView>>([]);
        const [pages, setPages] = useState<Array<{ data: ObjectColumnView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const objectColumns = response.data.pages.flatMap(x => x.data as ObjectColumnView[]);

                setMappedObjectColumns(objectColumns);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedObjectColumns,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },

    useGetObjectColumnById: (id: string) => {
        const query = `?$filter=id eq ${id}`
        const response = objectColumnService.useRetrieveAllObjectColumn(query)
        const [mappedObjectColumn, SetMappedObjectColumn] = useState<ObjectColumnView>();

        useEffect(() => {
            if (response.data && response.data[0]) {
                const objectColumn = new ObjectColumnView(
                    response.data[0].id,
                    response.data[0].specificationObjectId,
                    response.data[0].supplierColumnName,
                    response.data[0].ourColumnName,
                    response.data[0].columnDescription,
                    response.data[0].ordinalPosition,
                    response.data[0].populatedBy,
                    response.data[0].fhirDataType,
                    response.data[0].sqlDataType,
                    response.data[0].length,
                    response.data[0].precision,
                    response.data[0].scale,
                    response.data[0].supplierDateFormat,
                    response.data[0].isWatermark,
                    response.data[0].isSequencing,
                    response.data[0].isBusinessKey,
                    response.data[0].isUniqueRecordKey,
                    response.data[0].isVersionHashElement,
                    response.data[0].isSenderCode,
                    response.data[0].isAuthorCode,
                    response.data[0].isRelatedOrganisationId,
                    response.data[0].isDeleteFlag,
                    response.data[0].isPersonConfidentialData,
                    response.data[0].personConfidentialDataType,
                    response.data[0].maskingMethod,
                    response.data[0].isSensitiveRecordMarker,
                    response.data[0].codeSystem,
                    response.data[0].partitionColumnLevel,
                    response.data[0].createdBy,
                    response.data[0].createdDate,
                    response.data[0].updatedBy,
                    response.data[0].updatedDate,
                    response.data[0].specificationObject);

                SetMappedObjectColumn(objectColumn);
            }
        }, [response.data]);

        return {
            mappedObjectColumn, ...response
        }
    },

    useUpdateObjectColumn: () => {
        return objectColumnService.useModifyObjectColumn();
    },

    useRemoveObjectColumn: () => {
        return objectColumnService.useRemoveObjectColumn();
    },
}