import { useMsal } from "@azure/msal-react";;
import SpecificationObjectBroker from "../../brokers/apiBroker.specificationobjects";
import { SpecificationObject } from "../../models/specificationObjects/specificationObject";
import { useQueryClient, useMutation, useQuery, useInfiniteQuery } from "@tanstack/react-query";

export const specificationObjectService = {
    useCreateSpecificationObject: () => {
        const broker = new SpecificationObjectBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (specificationObject: SpecificationObject) => {
                const date = new Date();
                specificationObject.createdDate = specificationObject.updatedDate = date;
                specificationObject.createdBy = specificationObject.updatedBy = msal.accounts[0].username;

                return broker.PostSpecificationObjectAsync(specificationObject);
            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["SpecificationObjectGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SpecificationObjectGetById", { id: variables.id }] });
            }
        });
    },

    useRetrieveAllSpecificationObject: (query: string) => {
        const broker = new SpecificationObjectBroker();

        return useQuery({
            queryKey: ["SpecificationObjectGetAll", { query: query }],
            queryFn: () => broker.GetAllSpecificationObjectsAsync(query),
            staleTime: Infinity
        });
    },

    useRetrieveAllSpecificationObjectPages: (query: string) => {
        const broker = new SpecificationObjectBroker();

        return useInfiniteQuery({
            queryKey: ["SpecificationObjectGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetSpecificationObjectFirstPagesAsync(query)
                }
                return broker.GetSpecificationObjectSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useModifySpecificationObject: () => {
        const broker = new SpecificationObjectBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (specificationObject: SpecificationObject) => {
                const date = new Date();
                specificationObject.updatedDate = date;
                specificationObject.updatedBy = msal.accounts[0].username;

                return broker.PutSpecificationObjectAsync(specificationObject);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["SpecificationObjectGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SpecificationObjectGetById", { id: data.id }]});
            }
        });
    },

    useRemoveSpecificationObject: () => {
        const broker = new SpecificationObjectBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return broker.DeleteSpecificationObjectByIdAsync(id);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["SpecificationObjectGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["SpecificationObjectGetById", { id: data.id }] });
            }
        });
    },
}