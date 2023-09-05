import { useMsal } from "@azure/msal-react";
import { Guid } from "guid-typescript";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "react-query";
import ObjectColumnBroker from "../../brokers/apiBroker.objectcolumns";
import { ObjectColumn } from "../../models/objectColumns/objectColumn";

export const Service = {
    useCreateObjectColumn: () => {
        const broker = new ObjectColumnBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((objectColumn: ObjectColumn) => {
            const date = new Date();
            objectColumn.createdDate = objectColumn.updatedDate = date;
            objectColumn.createdBy = objectColumn.updatedBy = msal.accounts[0].username;

            return broker.PostObjectColumnAsync(objectColumn);
        },
            {
                onSuccess: (variables) => {
                    queryClient.invalidateQueries("ObjectColumnGetAll");
                    queryClient.invalidateQueries(["ObjectColumnGetById", { id: variables.id }]);
                }
            });
    },

    useRetrieveAllObjectColumn: (query: string) => {
        const broker = new ObjectColumnBroker();

        return useQuery(
            ["ObjectColumnGetAll", { query: query }],
            () => broker.GetAllObjectColumnsAsync(query),
            { staleTime: Infinity });
    },

    useRetrieveAllObjectColumnPages: (query: string) => {
        const broker = new ObjectColumnBroker();

        return useInfiniteQuery(
            ["ObjectColumnGetAll", { query: query }],
            ({ pageParam }) => {
                if (!pageParam) {
                    return broker.GetObjectColumnFirstPagesAsync(query)
                }
                return broker.GetObjectColumnSubsequentPagesAsync(pageParam)
            },
            {
                getNextPageParam: (lastPage) => lastPage.nextPage,
                staleTime: Infinity
            });
    },

    useModifyObjectColumn: () => {
        const broker = new ObjectColumnBroker();
        const queryClient = useQueryClient();
        const msal = useMsal();

        return useMutation((objectColumn: ObjectColumn) => {
            const date = new Date();
            objectColumn.updatedDate = date;
            objectColumn.updatedBy = msal.accounts[0].username;

            return broker.PutObjectColumnAsync(objectColumn);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("ObjectColumnGetAll");
                    queryClient.invalidateQueries(["ObjectColumnGetById", { id: data.id }]);
                }
            });
    },

    useRemoveObjectColumn: () => {
        const broker = new ObjectColumnBroker();
        const queryClient = useQueryClient();

        return useMutation((id: Guid) => {
            return broker.DeleteObjectColumnByIdAsync(id);
        },
            {
                onSuccess: (data) => {
                    queryClient.invalidateQueries("ObjectColumnGetAll");
                    queryClient.invalidateQueries(["ObjectColumnGetById", { id: data.id }]);
                }
            });
    },
}