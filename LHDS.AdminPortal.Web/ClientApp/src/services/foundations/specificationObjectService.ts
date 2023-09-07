import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import SpecificationObjectBroker from "../../brokers/apiBroker.specificationobjects";
import { SpecificationObject } from "../../models/specificationObjects/specificationObject";

export const Service = {
    useCreateSpecificationObject: () => {
        const broker = new SpecificationObjectBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((specificationObject: SpecificationObject) => {
            const date = new Date();
            specificationObject.createdDate = specificationObject.updatedDate = date;
            specificationObject.createdBy = specificationObject.updatedBy = msal.accounts[0].username;

            return broker.PostSpecificationObjectAsync(specificationObject);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("SpecificationObjectGetAll");
                    queryClient.invalidateQueries(["SpecificationObjectGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllSpecificationObject: (query: string) => {
        const broker = new SpecificationObjectBroker();

        return useQuery(
            ["SpecificationObjectGetAll", { query: query }],
            () => broker.GetAllSpecificationObjectsAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllSpecificationObjectPages: (query: string) => {
        const broker = new SpecificationObjectBroker();

        return useInfiniteQuery(
            ["SpecificationObjectGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetSpecificationObjectFirstPagesAsync(query)
                }
                return broker.GetSpecificationObjectSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifySpecificationObject: () => {
        const broker = new SpecificationObjectBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((specificationObject: SpecificationObject) => {
            const date = new Date();
            specificationObject.updatedDate = date;
            specificationObject.updatedBy = msal.accounts[0].username;

            return broker.PutSpecificationObjectAsync(specificationObject);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("SpecificationObjectGetAll");
                    queryClient.invalidateQueries(["SpecificationObjectGetById", { id: data.id }]);
                }
            });
    },

    useRemoveSpecificationObject: () => {
        const broker = new SpecificationObjectBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteSpecificationObjectByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("SpecificationObjectGetAll");
                    queryClient.invalidateQueries(["SpecificationObjectGetById", { id: data.id }]);
                }
            });
    },
}