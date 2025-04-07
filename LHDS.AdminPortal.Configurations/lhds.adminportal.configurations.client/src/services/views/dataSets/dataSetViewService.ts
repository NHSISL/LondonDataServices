import { useEffect, useState } from "react";
import { DataSetView } from "../../../models/views/components/dataSets/dataSetView";
import { dataSetService } from "../../foundations/dataSetService";
import { DataSet } from "../../../models/dataSets/dataSet";


type DataSetViewServiceResponse = {
    mappedDataSets: DataSetView[] | undefined;
    pages: Array<{ data: DataSetView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: DataSetView[] }> } | undefined;
    refetch: () => void
}

export const dataSetViewService = {
    useCreateDataSet: () => {
        return dataSetService.useCreateDataSet();
    },

    useGetAllDataSets: (searchTerm?: string): DataSetViewServiceResponse => {
        let query = `?$orderby=createdDate desc`;

        if (searchTerm) {
            query = query + `&$filter=contains(dataSetName,'${searchTerm}')`;
        }

        const response = dataSetService.useRetrieveAllDataSetPages(query);
        const [mappedDataSets, setMappedDataSets] = useState<Array<DataSetView>>();
        const [pages, setPages] = useState<Array<{ data: DataSetView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const dataSets: Array<DataSetView> = [];
                response.data.pages.forEach(x => {
                    x.data.forEach((dataSet: DataSet) => {
                        dataSets.push(new DataSetView(
                            dataSet.id,
                            dataSet.dataSetName,
                            dataSet.dataSetAliases,
                            dataSet.dataSetSupplier,
                            dataSet.dataSetAuthor,
                            dataSet.specifiedBy,
                            dataSet.IsNationallySpecified,
                            dataSet.collectedBy,
                            dataSet.isNationallyCollected,
                            dataSet.dataSourceType,
                            dataSet.isActive,
                            dataSet.activeFrom,
                            dataSet.activeTo,
                            dataSet.createdBy,
                            dataSet.createdDate,
                            dataSet.updatedBy,
                            dataSet.updatedDate,
                        ));
                    });
                });

                setMappedDataSets(dataSets);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedDataSets,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },

    useGetDataSetById: (id: string) => {
        const query = `?$filter=id eq ${id}`
        const response = dataSetService.useRetrieveAllDataSet(query)
        const [mappedDataSet, setMappedDataSet] = useState<DataSetView>();

        useEffect(() => {
            if (response.data && response.data[0]) {
                const dataSet = new DataSetView(
                    response.data[0].id,
                    response.data[0].dataSetName,
                    response.data[0].dataSetAliases,
                    response.data[0].dataSetSupplier,
                    response.data[0].dataSetAuthor,
                    response.data[0].specifiedBy,
                    response.data[0].IsNationallySpecified,
                    response.data[0].collectedBy,
                    response.data[0].isNationallyCollected,
                    response.data[0].dataSourceType,
                    response.data[0].isActive,
                    response.data[0].activeFrom,
                    response.data[0].activeTo,
                    response.data[0].createdBy,
                    response.data[0].createdDate,
                    response.data[0].updatedBy,
                    response.data[0].updatedDate);

                setMappedDataSet(dataSet);
            }
        }, [response.data]);

        return {
            mappedDataSet, ...response
        }
    },

    useUpdateDataSet: () => {
        return dataSetService.useModifyDataSet();
    },

    useRemoveDataSet: () => {
        return dataSetService.useRemoveDataSet();
    },
};
