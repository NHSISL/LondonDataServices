import { useEffect, useState } from "react";
import { pdsService } from "../../foundations/pdsService";
import { PdsHomeView } from "../../../models/pds/pdsHomeView";
import { Pds } from "../../../models/pds/pds";

type PdsHomeViewServiceResponse = {
    mappedPds: PdsHomeView[] | undefined;
    pages: any;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: any;
}

export const pdsHomeViewService = {
    useGetAllPds: (searchTerm?: string): PdsHomeViewServiceResponse => {

            let query = `?$orderby=createdDate`;

            if (searchTerm) {
                query = query + `&$filter=contains(message,'${searchTerm}') or contains(fileName,'${searchTerm}')`;
            }

            const response = pdsService.useGetAllPdsPages(query);
            const [mappedPds, setMappedPds] = useState<Array<PdsHomeView>>();;
            const [pages, setPages] = useState<any>([]);

            useEffect(() => {
                if (response.data && response.data.pages) {
                    const pdsArray: Array<PdsHomeView> = [];

                    response.data.pages.forEach((page: any) => {
                        page.data.forEach((pds: Pds) => {
                            pdsArray.push(new PdsHomeView(
                                pds.id,
                                pds.correlationId,
                                pds.message,
                                pds.fileName,
                                pds.messageId,
                                pds.createdDate,
                                pds.createdBy
                            ));
                        });
                    });

                    setMappedPds(pdsArray);
                    setPages(response.data.pages);
                }
            }, [response.data]);

            return {
                mappedPds,
                pages,
                isLoading: response.isLoading,
                fetchNextPage: response.fetchNextPage,
                isFetchingNextPage: response.isFetchingNextPage,
                hasNextPage: !!response.hasNextPage,
                data: response.data,
            };
    },
};
