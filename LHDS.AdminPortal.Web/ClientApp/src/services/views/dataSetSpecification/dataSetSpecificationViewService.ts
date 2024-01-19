import { useEffect, useState } from "react";
import { Guid } from "guid-typescript";
import { DataSetSpecificationView } from "../../../models/views/components/dataSetSpecifications/dataSetSpecificationView";
import { dataSetSpecificationService } from "../../foundations/dataSetSpecificationService";
import { DataSetSpecification } from "../../../models/dataSetSpecifications/dataSetSpecification";


type dataSetSpecificationViewServiceResponse = {
    mappedDataSetSpecifications: DataSetSpecificationView[] | undefined;
    pages: any;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: any;
    refetch: () => void
}

export const dataSetSpecificationViewService = {
    useCreateDataSetSpecification: () => {
        return dataSetSpecificationService.useCreateDataSetSpecification();
    },

    useGetAllDataSetSpecifications: (dataSetId: string, searchTerm?: string,): dataSetSpecificationViewServiceResponse => {
        try {
            let query = `?$filter=dataSetId eq ${dataSetId}&$expand=dataSet&$orderby=createdDate desc`;

            if (searchTerm) {
                query = query + `&$filter=contains(dataSetName,'${searchTerm}')`;
            }

            const response = dataSetSpecificationService.useRetrieveAllDataSetSpecificationPages(query);

            const [mappedDataSetSpecifications, setMappedDataSetSpecifications] =
                useState<Array<DataSetSpecificationView>>();

            const [pages, setPages] = useState<any>([]);

            useEffect(() => {
                if (response.data && response.data.pages) {
                    const dataSetSpecifications: Array<DataSetSpecificationView> = [];
                    response.data.pages.forEach(x => {
                        x.data.forEach((dataSetSpecification: DataSetSpecification) => {
                            dataSetSpecifications.push(new DataSetSpecificationView(
                                dataSetSpecification.id,
                                dataSetSpecification.dataSetId,
                                dataSetSpecification.supplierSpecificationVersion,
                                dataSetSpecification.isPublished,
                                dataSetSpecification.isMultiAuthorPerBatch,
                                dataSetSpecification.isActive,
                                dataSetSpecification.ourSpecificationVersion,
                                dataSetSpecification.notes,
                                dataSetSpecification.entityChangeSynchronisation,
                                dataSetSpecification.dateReleased,
                                dataSetSpecification.dateImplementation,
                                dataSetSpecification.dateSuperseded,
                                dataSetSpecification.supersededById,
                                dataSetSpecification.presededById,
                                dataSetSpecification.activeFrom,
                                dataSetSpecification.activeTo,
                                dataSetSpecification.createdBy,
                                dataSetSpecification.createdDate,
                                dataSetSpecification.updatedBy,
                                dataSetSpecification.updatedDate,
                                dataSetSpecification.dataSet
                            ));
                        });
                    });

                    setMappedDataSetSpecifications(dataSetSpecifications);
                    setPages(response.data.pages);
                }
            }, [response.data]);

            return {
                mappedDataSetSpecifications,
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

    useGetDataSetSpecificationById: (id: Guid) => {
        try {
            const query = `?$filter=id eq ${id}`
            const response = dataSetSpecificationService.useRetrieveAllDataSetSpecification(query)
            const [mappedDataSetSpecification, setMappedDataSetSpecification] = useState<DataSetSpecificationView>();

            useEffect(() => {
                if (response.data && response.data[0]) {
                    const dataSetSpecification = new DataSetSpecificationView(
                        response.data[0].id,
                        response.data[0].dataSetId,
                        response.data[0].supplierSpecificationVersion,
                        response.data[0].isPublished,
                        response.data[0].isMultiAuthorPerBatch,
                        response.data[0].isActive,
                        response.data[0].ourSpecificationVersion,
                        response.data[0].notes,
                        response.data[0].entityChangeSynchronisation,
                        response.data[0].dateReleased,
                        response.data[0].dateImplementation,
                        response.data[0].dateSuperseded,
                        response.data[0].supersededById,
                        response.data[0].presededById,
                        response.data[0].activeFrom,
                        response.data[0].activeTo,
                        response.data[0].createdBy,
                        response.data[0].createdDate,
                        response.data[0].updatedBy,
                        response.data[0].updatedDate);

                    setMappedDataSetSpecification(dataSetSpecification);
                }
            }, [response.data]);

            return {
                mappedDataSetSpecification, ...response
            }
        } catch (err) {
            throw err;
        }
    },

    useUpdateDataSetSpecification: () => {
        return dataSetSpecificationService.useModifyDataSetSpecification();
    },

    useRemoveDataSetSpecification: () => {
        return dataSetSpecificationService.useRemoveDataSetSpecification();
    },
};
