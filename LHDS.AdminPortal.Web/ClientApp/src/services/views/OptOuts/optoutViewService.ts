import { useEffect, useState } from "react";
import { OptOut } from "../../../models/optout/optout";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import { optOutService } from "../../foundations/optoutService";

export const optOutViewService = {

    useCreateOptOut: () => {
        return optOutService.useCreateOptOut();
    },

    useGetAllOptOuts: (searchTerm?: string) => {
        try {

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
                            optOut.uniqueReference,
                            optOut.batchReference,
                            optOut.cacheTime,
                            optOut.lastSentToMesh,
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
        } catch (err) 
        {
            throw err;
        }
    },

    useGetOptOutsByNhsNumber: (nhsNumber: string) => {
        try {
            const query = `/${nhsNumber}`
            const response = optOutService.useGetAllOptOuts(query);
            const [mappedOptOut, setMappedOptOut] = useState<OptOutView>();

            useEffect(() => {
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

                    setMappedOptOut(optOut);
                }
            }, [response.data]);

            return {
                mappedOptOut, ...response
            }
        } catch (err) 
        {
            throw err;
        }
    },

    useUpdateSupplier: () => {
        return optOutService.useUpdateOptOut();
    },

    useRemoveSupplier: () => {
        return optOutService.useDeleteOptOut();
    },
}