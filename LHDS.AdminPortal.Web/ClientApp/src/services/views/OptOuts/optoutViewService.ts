import { useEffect, useState } from "react";
import { OptOut } from "../../../models/optout/optout";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import { optOutService } from "../../foundations/optoutService";


export const optOutViewService = {

    useCreateOptOut: () => {
        return optOutService.useCreateOptOut();
    },

    useGetAllOptOuts: () => {
        try {
            let query = '?$orderby=name/createdDate';

            const response = optOutService.useGetAllOptOuts(query);
            const [mappedOptOuts, setMappedOptOuts] = useState<Array<OptOutView>>([]);

            useEffect(() => {
                if (response.data) {
                    const optOuts = response.data.map((optOut: OptOut) =>
                        new OptOutView(
                            optOuts.id,
                            optOuts.nhsNumber,
                            optOuts.optOutStatus,
                            optOuts.cacheTime,
                            optOuts.lastSentToMesh,
                            optOuts.createdBy,
                            optOuts.createdDate,
                            optOuts.updatedBy,
                            optOuts.updatedDate,
                        ));

                    setMappedOptOuts(optOuts);
                }
            }, [response.data]);

            return {
                mappedOptOuts, ...response
            }
        } catch (err) {
            throw err;
        }
    },

    useGetOptOutsByNhsNumber: (nhsNumber: string) => {
        try {
            const query = `?$filter=nhsNumber eq ${nhsNumber}`
            const response = optOutService.useGetOptOutByNhsNumber(query);
            const [mappedOptOut, setMappedOptOut] = useState<OptOutView>();

            useEffect(() => {
                if (response.data) {
                    const optOut = new OptOutView(
                        response.data.id,
                        response.data.nhsNumber,
                        response.data.optOutStatus,
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
        } catch (err) {
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