import { useMsal } from "@azure/msal-react";
import ObjectColumnBroker from "../../brokers/apiBroker.objectcolumns";
import { ObjectColumn } from "../../models/objectColumns/objectColumn";
import { useQueryClient, useMutation, useQuery, useInfiniteQuery } from "@tanstack/react-query";

export const objectColumnService = {
    useCreateObjectColumn: () => {
        const broker = new ObjectColumnBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (objectColumn: ObjectColumn) => {
                const date = new Date();
                objectColumn.createdDate = objectColumn.updatedDate = date;
                objectColumn.createdBy = objectColumn.updatedBy = msal.accounts[0].username;

                return broker.PostObjectColumnAsync(objectColumn);
            },
            onSuccess: (variables) => {
                queryClient.invalidateQueries({ queryKey: ["ObjectColumnGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["ObjectColumnGetById", { id: variables.id }]});
            }
        });
    },

    useRetrieveAllObjectColumn: (query: string) => {
        const broker = new ObjectColumnBroker();

        return useQuery({
            queryKey: ["ObjectColumnGetAll", { query: query }],
            queryFn: () => broker.GetAllObjectColumnsAsync(query),
            staleTime: Infinity
        });
    },

    useRetrieveAllObjectColumnPages: (query: string) => {
        const broker = new ObjectColumnBroker();

        return useInfiniteQuery({
            queryKey: ["ObjectColumnGetAll", { query: query }],
            queryFn: ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetObjectColumnFirstPagesAsync(query)
                }
                return broker.GetObjectColumnSubsequentPagesAsync(pageParam)
            },
            initialPageParam: "",
            staleTime: Infinity,
            getNextPageParam: (lastPage) => lastPage.nextPage ?? null,
        });
    },

    useModifyObjectColumn: () => {
        const broker = new ObjectColumnBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation({
            mutationFn: (objectColumn: ObjectColumn) => {
                const date = new Date();
                objectColumn.updatedDate = date;
                objectColumn.updatedBy = msal.accounts[0].username;

                return broker.PutObjectColumnAsync(objectColumn);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["ObjectColumnGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["ObjectColumnGetById", { id: data.id }] });
            }
        });
    },

    useRemoveObjectColumn: () => {
        const broker = new ObjectColumnBroker();
        const queryClient = useQueryClient();

        return useMutation({
            mutationFn: (id: string) => {
                return broker.DeleteObjectColumnByIdAsync(id);
            },
            onSuccess: (data) => {
                queryClient.invalidateQueries({ queryKey: ["ObjectColumnGetAll"] });
                queryClient.invalidateQueries({ queryKey: ["ObjectColumnGetById", { id: data.id }] });
            }
        });
    },
}