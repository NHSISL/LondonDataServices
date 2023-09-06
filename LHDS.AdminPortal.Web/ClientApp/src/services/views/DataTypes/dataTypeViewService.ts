import { Guid } from "guid-typescript";
import { useEffect, useState } from "react";
import { DataTypeService } from "../../foundations/dataTypeService";
import { DataTypeView } from "../../../models/views/components/dataTypes/dataTypeView";
import { DataType } from "../../../models/dataTypes/dataType";

export const DataTypeViewService = {

    useCreateDataType: () => {
        return DataTypeService.useCreateDataType();
    },

    useGetAllDataTypes: (searchTerm?: string) => {
        try {
            let query = '?$orderby=name';

            if (searchTerm) {
                query = query + `&$filter=contains(name,'${searchTerm}')`;
            }

            const response = DataTypeService.useRetrieveAlldataType(query);
            const [mappedDataTypes, setMappedDataTypes] = useState<Array<DataTypeView>>([]);

            useEffect(() => {
                if (response.data) {
                    const dataTypes = response.data.map((dataType: DataType) =>
                        new DataTypeView(
                            dataType.id,
                            dataType.name,
                            dataType.createdBy,
                            dataType.createdDate,
                            dataType.updatedBy,
                            dataType.updatedDate,
                        ));

                    setMappedDataTypes(dataTypes);
                }
            }, [response.data]);

            return {
                mappedDataTypes, ...response
            }
        } catch (err) {
            throw err;
        }
    },

    useUpdateDataType: () => {
        return DataTypeService.useModifyDataType();
    },

    useRemoveDataType: () => {
        return DataTypeService.useRemoveDataType();
    },
}