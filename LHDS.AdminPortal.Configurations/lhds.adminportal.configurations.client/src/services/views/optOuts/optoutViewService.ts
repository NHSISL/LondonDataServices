import { useEffect, useState } from "react";
import { OptOut } from "../../../models/optout/optout";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import { optOutService } from "../../foundations/optoutService";

export const optOutViewService = {

    useCreateOptOut: () => {
        return optOutService.useCreateOptOut();
    },

    useGetAllOptOuts: (searchTerm?: string) => {
        let query = ``;

        if (searchTerm) {
            query = query + `?$orderby=createdDate&$filter=nhsNumber eq '${searchTerm}'`;
        }

        const response = optOutService.useGetAllOptOuts(query);
        const [mappedOptOuts, setMappedOptOuts] = useState<Array<OptOutView>>([]);

        useEffect(() => {
            if (response.data && response.data) {
                const optOuts = response.data.map((optOut: OptOut) => new OptOutView(
                    optOut.id,
                    optOut.nhsNumber,
                    optOut.status,
                    optOut.cacheTime,
                    optOut.lastSentToMesh,
                    optOut.uniqueReference,
                    optOut.batchReference,
                    optOut.createdBy,
                    optOut.createdDate,
                    optOut.updatedBy,
                    optOut.updatedDate,
                ));

                setMappedOptOuts(optOuts);
            }
        }, [response.data]);

        return {
            mappedOptOuts, ...response
        }
    },

    useGetOptOutsByNhsNumber: (nhsNumber: string) => {

        const response = optOutService.useGetOptOutById(nhsNumber);
        const [mappedOptOut, setMappedOptOut] = useState<OptOutView>();

        useEffect(() => {
            if (response.data) {
                const optOut = new OptOutView(
                    response.data.id,
                    response.data.nhsNumber,
                    response.data.status,
                    response.data.cacheTime,
                    response.data.lastSentToMesh,
                    response.data.uniqueReference,
                    response.data.batchReference,
                    response.data.createdBy,
                    response.data.createdDate,
                    response.data.updatedBy,
                    response.data.updatedDate
                );

                setMappedOptOut(optOut);
            } else {
                setMappedOptOut(undefined);
            }
        }, [response.data]);

        return {
            mappedOptOut, ...response
        }
    },

    useGetOptOutsByNhsNumbers: (nhsNumbers: string[]) => {
        const [matchedOptOuts, setMatchedOptOuts] = useState<OptOutView[]>([]);

        useEffect(() => {
            const fetchData = async () => {
                const fetchedOptOuts: OptOutView[] = [];

                for (const nhsNumber of nhsNumbers) {
                    try {
                        const query = `/${nhsNumber}`;
                        const response = await optOutService.useGetAllOptOuts(query);

                        if (response.data) {
                            const optOut = new OptOutView(
                                response.data.id,
                                response.data.nhsNumber,
                                response.data.status,
                                response.data.uniqueReference,
                                response.data.batchReference,
                                response.data.cacheTime,
                                response.data.lastSentToMesh,
                                response.data.createdBy,
                                response.data.createdDate,
                                response.data.updatedBy,
                                response.data.updatedDate
                            );

                            fetchedOptOuts.push(optOut);
                        }
                    } catch (error) {
                        console.error(`Error fetching opt-out for NHS number ${nhsNumber}:`, error);
                    }
                }

                setMatchedOptOuts(fetchedOptOuts);
            };

            fetchData();
        }, [nhsNumbers]);

        return matchedOptOuts;
    },

    useUpdateOptOut: () => {
        return optOutService.useUpdateOptOut();
    },

    useRemoveOptOut: () => {
        return optOutService.useDeleteOptOut();
    },
}