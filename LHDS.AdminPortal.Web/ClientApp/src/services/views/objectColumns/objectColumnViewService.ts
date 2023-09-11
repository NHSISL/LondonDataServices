import { useEffect, useState } from "react";
import { ObjectColumnView } from "../../../models/views/components/objectColumns/objectColumnView";
import { ObjectColumn } from "../../../models/objectColumns/objectColumn";
import { objectColumnService } from "../../foundations/objectColumnService";

export const objectColumnViewService = {

    useCreateObjectColumn: () => {
        return objectColumnService.useCreateObjectColumn();
    },

    useGetAllObjectColumns: (searchTerm?: string) => {
        try {
            let query = '?$orderby=name';

            if (searchTerm) {
                query = query + `&$filter=contains(name,'${searchTerm}')`;
            }

            const response = objectColumnService.useRetrieveAllObjectColumn(query);
            const [mappedObjectColumns, setMappedObjectColumns] = useState<Array<ObjectColumnView>>([]);

            useEffect(() => {
                if (response.data) {
                    const objectColumns = response.data.map((objectColumn: ObjectColumn) =>
                        new ObjectColumnView(
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
                            objectColumn.isEntityBusinessKey,
                            objectColumn.isRecordBusinessKey,
                            objectColumn.isVersionHashElement,
                            objectColumn.isSenderCode,
                            objectColumn.isAuthorCode,
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

                    setMappedObjectColumns(objectColumns);
                }
            }, [response.data]);

            return {
                mappedObjectColumns, ...response
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