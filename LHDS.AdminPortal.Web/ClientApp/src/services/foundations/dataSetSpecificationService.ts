import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import DataSetSpecificationBroker from "../../brokers/apiBroker.datasetspecifications";
import { DataSetSpecification } from "../../models/dataSetSpecifications/dataSetSpecification";

export const Service = {
    useCreateDataSetSpecification: () => {
        const broker = new DataSetSpecificationBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((dataSetSpecification: DataSetSpecification) => {
            const date = new Date();
            dataSetSpecification.createdDate = dataSetSpecification.updatedDate = date;
            dataSetSpecification.createdBy = dataSetSpecification.updatedBy = msal.accounts[0].username;

            return broker.PostDataSetSpecificationAsync(dataSetSpecification);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("DataSetSpecificationGetAll");
                    queryClient.invalidateQueries(["DataSetSpecificationGetById", { id: variables.id }]);
                }
            });
    },
}