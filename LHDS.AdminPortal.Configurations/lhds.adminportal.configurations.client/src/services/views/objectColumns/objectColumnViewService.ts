import { useEffect, useState } from "react";
import { ObjectColumnView } from "../../../models/views/components/objectColumns/objectColumnView";
import { ObjectColumn } from "../../../models/objectColumns/objectColumn";
import { objectColumnService } from "../../foundations/objectColumnService";
import { Guid } from "guid-typescript";

type ObjectColumnViewServiceResponse = {
    mappedObjectColumns: ObjectColumnView[] | undefined;
    pages: any;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: any;
    refetch: () => void
}

export const objectColumnViewService = {
    useCreateObjectColumn: () => {
        return objectColumnService.useCreateObjectColumn();
    },

    useGetAllObjectColumns: (specificationObjectId: string, searchTerm?: string): ObjectColumnViewServiceResponse => {
        try {
            let query = `?$filter=specificationObjectId eq ${specificationObjectId}&$expand=SpecificationObject&$orderby=createdDate desc`;

            if (searchTerm) {
                query = query + `&$filter=contains(ourObjectName,'${searchTerm}')`;
            }

            const response = objectColumnService.useRetrieveAllObjectColumnPages(query);
            const [mappedObjectColumns, setMappedObjectColumns] = useState<Array<ObjectColumnView>>([]);
            const [pages, setPages] = useState<any>([]);

            useEffect(() => {
                if (response.data && response.data.pages) {
                    const objectColumns: Array<ObjectColumnView> = [];
                    response.data.pages.forEach(x => {
                        x.data.forEach((objectColumn: ObjectColumn) => {
                            objectColumns.push(new ObjectColumnView(
                                objectColumn.id,
                                objectColumn.specificationObjectId,
                                objectColumn.supplierColumnName,
                                objectColumn.ourColumnName,
                                objectColumn.columnDescription,
                                objectColumn.ordinalPosition,
                                objectColumn.populatedBy,
                                objectColumn.fhirDataType,
                                objectColumn.sqlDataType,
                                objectColumn.length,
                                objectColumn.precision,
                                objectColumn.scale,
                                objectColumn.supplierDateFormat,
                                objectColumn.isWatermark,
                                objectColumn.isSequencing,
                                objectColumn.isBusinessKey,
                                objectColumn.isUniqueRecordKey,
                                objectColumn.isVersionHashElement,
                                objectColumn.isSenderCode,
                                objectColumn.isAuthorCode,
                                objectColumn.isRelatedOrganisationId,
                                objectColumn.isDeleteFlag,
                                objectColumn.isPersonConfidentialData,
                                objectColumn.personConfidentialDataType,
                                objectColumn.maskingMethod,
                                objectColumn.isSensitiveRecordMarker,
                                objectColumn.codeSystem,
                                objectColumn.partitionColumnLevel,
                                objectColumn.createdBy,
                                objectColumn.createdDate,
                                objectColumn.updatedBy,
                                objectColumn.updatedDate,
                                objectColumn.specificationObject
                            ));
                        });
                    });

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
        } catch (err) {
            throw err;
        }
    },

    useGetObjectColumnById: (id: Guid) => {
        try {
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
        } catch (err) {
            throw err;
        }
    },

    useUpdateObjectColumn: () => {
        return objectColumnService.useModifyObjectColumn();
    },

    useRemoveObjectColumn: () => {
        return objectColumnService.useRemoveObjectColumn();
    },
}