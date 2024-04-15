import { useEffect, useState } from "react";
import { DataTypeView } from "../../../models/views/components/dataTypes/dataTypeView";
import { DataType } from "../../../models/dataTypes/dataType";
import { dataTypeService } from "../../foundations/dataTypeService";

export const dataTypeViewService = {

    useCreateDataType: () => {
        return dataTypeService.useCreateDataType();
    },

    useGetAllDataTypes: (searchTerm?: string) => {
        try {
            let query = '?$orderby=Name';

            if (searchTerm) {
                query = query + `&$filter=contains(Name,'${searchTerm}')`;
            }

            const response = dataTypeService.useRetrieveAllDataType(query);
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
        return dataTypeService.useModifyDataType();
    },

    useRemoveDataType: () => {
        return dataTypeService.useRemoveDataType();
    },
}