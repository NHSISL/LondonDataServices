import { useEffect, useState } from "react";
import { resolvedAddressService } from "../../foundations/resolvedAddressService";
import { ResolvedAddressHomeView } from "../../../models/resolvedAddresses/resolvedAddressHomeView";
import { ResolvedAddress } from "../../../models/resolvedAddresses/resolvedAddress";

type ResolvedAddressHomeViewServiceResponse = {
    mappedResolvedAddresses: ResolvedAddressHomeView[] | undefined;
    pages: any;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: any;
    refetch: () => void
}

export const ResolvedAddressHomeViewService = {
    useGetAllResolvedAddresses: (searchTerm?: string): ResolvedAddressHomeViewServiceResponse => {
        try 
        {
            let query = ``;
            query = `?$orderby=createdDate desc`;

            if (searchTerm) {
                const fields = [
                    'uprn',
                    'unstructuredPostalAddress'
                ];

                const filterConditions = fields
                    .map(field => `contains(${field},'${searchTerm}')`)
                    .join(' or ');

                query = query + `&$filter=${filterConditions}`;
            }

            const response = resolvedAddressService.useRetrieveAllResolvedAddressPages(query);

            const [mappedResolvedAddresses, setMappedResolvedAddresses] =
                useState<Array<ResolvedAddressHomeView>>();

            const [pages, setPages] = useState<any>([]);

            useEffect(() => {
                if (response.data && response.data.pages) {
                    const resolvedAddressArray: Array<ResolvedAddressHomeView> = [];

                    response.data.pages.forEach((page: any) => {
                        page.data.forEach((resolvedAddress: ResolvedAddress) => {
                            resolvedAddressArray.push(new ResolvedAddressHomeView(
                                resolvedAddress.id,
                                resolvedAddress.uprn,
                                resolvedAddress.upsn,
                                resolvedAddress.postCode,
                                resolvedAddress.retryCount,
                                resolvedAddress.isProcessing,
                                resolvedAddress.uniqueReference,
                                resolvedAddress.unstructuredPostalAddress,
                                resolvedAddress.alternateUnstructuredPostalAddress ?? undefined,
                                resolvedAddress.batchReference,
                                resolvedAddress.addressFormatQuality,
                                resolvedAddress.algorithm,
                                resolvedAddress.buildingName,
                                resolvedAddress.buildingNumber,
                                resolvedAddress.classification,
                                resolvedAddress.departmentName,
                                resolvedAddress.dependentLocality,
                                resolvedAddress.dependentThoroughfare,
                                resolvedAddress.doubleDependentLocality,
                                resolvedAddress.matchPattern,
                                resolvedAddress.matchedWithAssign,
                                resolvedAddress.organisationName,
                                resolvedAddress.postCodeQuality,
                                resolvedAddress.postTown,
                                resolvedAddress.qualifier,
                                resolvedAddress.subBuildingName,
                                resolvedAddress.thoroughfare,
                                resolvedAddress.isExported,
                                resolvedAddress.isProcessed,
                                resolvedAddress.createdBy,
                                resolvedAddress.createdDate,
                                resolvedAddress.updatedBy,
                                resolvedAddress.updatedDate
                            ));
                        });
                    });

                    setMappedResolvedAddresses(resolvedAddressArray);
                    setPages(response.data.pages);
                }
            }, [response.data]);

            return {
                mappedResolvedAddresses,
                pages,
                isLoading: response.isLoading,
                fetchNextPage: response.fetchNextPage,
                isFetchingNextPage: response.isFetchingNextPage,
                hasNextPage: !!response.hasNextPage,
                data: response.data,
                refetch: response.refetch
            };
        } catch (err) 
        {
            throw err;
        }
    },
};
