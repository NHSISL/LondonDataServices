import { useEffect, useState } from "react";
import { resolvedAddressService } from "../../foundations/resolvedAddressService";
import { ResolvedAddressHomeView } from "../../../models/resolvedAddresses/resolvedAddressHomeView";
import { ResolvedAddress } from "../../../models/resolvedAddresses/resolvedAddress";

type ResolvedAddressHomeViewServiceResponse = {
    mappedResolvedAddresses: ResolvedAddressHomeView[] | undefined;
    pages: Array<{ data: ResolvedAddressHomeView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: ResolvedAddressHomeView[] }> } | undefined;
    refetch: () => void;
};

export const ResolvedAddressHomeViewService = {
    useGetAllResolvedAddresses: (
        searchTerm?: string,
        matchedFilterParam?: boolean
    ): ResolvedAddressHomeViewServiceResponse => {
        const filters: string[] = [];

        if (matchedFilterParam === true) {
            filters.push(`MatchedWithAssign eq true`);
        } else if (matchedFilterParam === false) {
            filters.push(`MatchedWithAssign eq false`);
        }

        if (searchTerm) {
            const searchFilter = [
                `contains(uprn,'${searchTerm}')`,
                `contains(unstructuredPostalAddress,'${searchTerm}')`
            ].join(" or ");
            filters.push(`(${searchFilter})`);
        }

        const filterQuery = filters.length ? `$filter=${filters.join(" and ")}` : "";
        const orderByQuery = `$orderby=createdDate desc`;
        const query = `?${filterQuery}&${orderByQuery}`;

        const response = resolvedAddressService.useRetrieveAllResolvedAddressPages(query);

        const [mappedResolvedAddresses, setMappedResolvedAddresses] =
            useState<Array<ResolvedAddressHomeView>>();
        const [pages, setPages] = useState<Array<{ data: ResolvedAddressHomeView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const resolvedAddressArray: Array<ResolvedAddressHomeView> = [];
                response.data.pages.forEach(x => {
                    x.data.forEach((resolvedAddress: ResolvedAddress) => {
                        resolvedAddressArray.push(
                            new ResolvedAddressHomeView(
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
                            )
                        );
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
    }
};