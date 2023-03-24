import { Guid } from "guid-typescript";
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
                //query = `?$filter=id eq 508158d9-70d3-4771-baae-c7e93775512d`
            }

            const response = optOutService.useGetAllOptOuts(query);
            const [mappedOptOuts, setMappedOptOuts] = useState<Array<OptOutView>>([]);

            useEffect(() => {
                if (response.data) {
                    const optOuts = response.data.map((optOut: OptOut) => new OptOutView(
                            optOut.id,
                            optOut.nhsNumber,
                            optOut.optOutStatus,
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
        } catch (err) {
            throw err;
        }
    },

    useGetOptOutsById: (id: Guid) => {
        try {
            const query = `?$filter=id eq ${id}`
            const response = optOutService.useGetAllOptOuts(query);
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