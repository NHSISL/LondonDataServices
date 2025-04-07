import { useEffect, useState } from "react";
import { SpecificationObjectView } from "../../../models/views/components/specificationObjects/specificationObjectView";
import { specificationObjectService } from "../../foundations/specificationObjectService";
import { SpecificationObject } from "../../../models/specificationObjects/specificationObject";

type SpecificationObjectViewServiceResponse = {
    mappedSpecificationObjects: SpecificationObjectView[] | undefined;
    pages: Array<{ data: SpecificationObjectView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: SpecificationObjectView[] }> } | undefined;
    refetch: () => void
}

export const specificationObjectViewService = {
    useCreateSpecificationObject: () => {
        return specificationObjectService.useCreateSpecificationObject();
    },

    useGetAllSpecificationObjects: (dataSetSpecificationId: string, searchTerm?: string): SpecificationObjectViewServiceResponse => {
        let query = `?$filter=dataSetSpecificationId eq ${dataSetSpecificationId}&$expand=dataSetSpecification&$orderby=createdDate desc`;

        if (searchTerm) {
            query = query + `&$filter=contains(specificationObjectName,'${searchTerm}')`;
        }

        const response = specificationObjectService.useRetrieveAllSpecificationObjectPages(query);
        const [mappedSpecificationObjects, SetMappedSpecificationObjects] = useState<Array<SpecificationObjectView>>();
        const [pages, setPages] = useState<Array<{ data: SpecificationObjectView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const specificationObjects: Array<SpecificationObjectView> = [];
                response.data.pages.forEach(x => {
                    x.data.forEach((specificationObject: SpecificationObject) => {
                        specificationObjects.push(new SpecificationObjectView(
                            specificationObject.id,
                            specificationObject.dataSetSpecificationId,
                            specificationObject.supplierObjectName,
                            specificationObject.ourObjectName,
                            specificationObject.objectDescription,
                            specificationObject.interchangeProtocol,
                            specificationObject.isPushedToUs,
                            specificationObject.isPulledByUs,
                            specificationObject.deletionHandling,
                            specificationObject.isSubmissionHeaderObject,
                            specificationObject.isTransactionLog,
                            specificationObject.createdBy,
                            specificationObject.createdDate,
                            specificationObject.updatedBy,
                            specificationObject.updatedDate,
                            specificationObject.dataSetSpecification
                        ));
                    });
                });

                SetMappedSpecificationObjects(specificationObjects);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedSpecificationObjects,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },

    useGetSpecificationObjectById: (id: string) => {
        const query = `?$filter=id eq ${id}`
        const response = specificationObjectService.useRetrieveAllSpecificationObject(query)
        const [mappedSpecificationObject, SetMappedSpecificationObject] = useState<SpecificationObjectView>();

        useEffect(() => {
            if (response.data && response.data[0]) {
                const specificationObject = new SpecificationObjectView(
                    response.data[0].id,
                    response.data[0].dataSetSpecificationId,
                    response.data[0].supplierObjectName,
                    response.data[0].ourObjectName,
                    response.data[0].objectDescription,
                    response.data[0].interchangeProtocol,
                    response.data[0].isPushedToUs,
                    response.data[0].isPulledByUs,
                    response.data[0].deletionHandling,
                    response.data[0].isSubmissionHeaderObject,
                    response.data[0].isTransactionLog,
                    response.data[0].createdBy,
                    response.data[0].createdDate,
                    response.data[0].updatedBy,
                    response.data[0].updatedDate,
                    response.data[0].dataSetSpecification);

                SetMappedSpecificationObject(specificationObject);
            }
        }, [response.data]);

        return {
            mappedSpecificationObject, ...response
        }
    },

    useUpdateSpecificationObject: () => {
        return specificationObjectService.useModifySpecificationObject();
    },

    useRemoveSpecificationObject: () => {
        return specificationObjectService.useRemoveSpecificationObject();
    },
};
