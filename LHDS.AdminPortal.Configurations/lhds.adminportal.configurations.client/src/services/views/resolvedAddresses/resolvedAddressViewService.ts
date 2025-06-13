import { Guid } from "guid-typescript";
import { useEffect, useState } from "react";
import { resolvedAddressService } from "../../foundations/resolvedAddressService";
import { ResolvedAddressView } from "../../../models/views/components/resolvedAddresses/resolvedAddressView";
import { ResolvedAddress } from "../../../models/resolvedAddresses/resolvedAddress";


export const resolvedAddressViewService = {

    useCreateResolvedAddress: () => {
        return resolvedAddressService.useCreateResolvedAddress();
    },

    useGetAllResolvedAddresses: (searchTerm?: string) => {

        let query = '?$orderby=name';

        if (searchTerm) {
            const fields = [
                'uprn',
                'upsn',
                'unstructuredPostalAddress',
                'alternateUnstructuredPostalAddress',
                'addressFormatQuality',
                'algorithm',
                'buildingName',
                'buildingNumber',
                'classification',
                'departmentName',
                'dependentLocality',
                'dependentThoroughfare',
                'doubleDependentLocality',
                'matchPattern',
                'organisationName',
                'postCodeQuality',
                'postTown',
                'qualifier',
                'subBuildingName',
                'thoroughfare'
            ];

            const filterConditions = fields
                .map(field => `contains(${field},'${searchTerm}')`)
                .join(' or ');

            query = query + `&$filter=${filterConditions}`;
        }

        const response = resolvedAddressService.useRetrieveAllResolvedAddresses(query);
        const [mappedResolvedAddresses, setMappedResolvedAddresses] = useState<Array<ResolvedAddressView>>([]);

        useEffect(() => {
            if (response.data) {
                const resolvedAddresses =
                    response.data.map((resolvedAddress: ResolvedAddress) => new ResolvedAddressView(
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

                setMappedResolvedAddresses(resolvedAddresses);
            }
        }, [response.data]);

        return {
            mappedResolvedAddresses, ...response
        }
    },

    useGetResolvedAddressById: (id: Guid) => {
        const query = `?$filter=id eq ${id}`
        const response = resolvedAddressService.useRetrieveAllResolvedAddresses(query);
        const [mappedResolvedAddress, setMappedResolvedAddress] = useState<ResolvedAddressView>();

        useEffect(() => {
            if (response.data && response.data[0]) {
                const resolvedAddress = new ResolvedAddressView(
                    response.data[0].id,
                    response.data[0].uprn,
                    response.data[0].upsn,
                    response.data[0].postCode,
                    response.data[0].retryCount,
                    response.data[0].isProcessing,
                    response.data[0].uniqueReference,
                    response.data[0].unstructuredPostalAddress,
                    response.data[0].alternateUnstructuredPostalAddress,
                    response.data[0].batchReference,
                    response.data[0].addressFormatQuality,
                    response.data[0].algorithm,
                    response.data[0].buildingName,
                    response.data[0].buildingNumber,
                    response.data[0].classification,
                    response.data[0].departmentName,
                    response.data[0].dependentLocality,
                    response.data[0].dependentThoroughfare,
                    response.data[0].doubleDependentLocality,
                    response.data[0].matchPattern,
                    response.data[0].matchedWithAssign,
                    response.data[0].organisationName,
                    response.data[0].postCodeQuality,
                    response.data[0].postTown,
                    response.data[0].qualifier,
                    response.data[0].subBuildingName,
                    response.data[0].thoroughfare,
                    response.data[0].isExported,
                    response.data[0].isProcessed,
                    response.data[0].createdBy,
                    response.data[0].createdDate,
                    response.data[0].updatedBy,
                    response.data[0].updatedDate
                );

                setMappedResolvedAddress(resolvedAddress);
            }
        }, [response.data]);

        return {
            mappedResolvedAddress, ...response
        }
    },

    useUpdateResolvedAddress: () => {
        return resolvedAddressService.useModifyResolvedAddress();
    },

    useRemoveResolvedAddress: () => {
        return resolvedAddressService.useRemoveResolvedAddress();
    },
}