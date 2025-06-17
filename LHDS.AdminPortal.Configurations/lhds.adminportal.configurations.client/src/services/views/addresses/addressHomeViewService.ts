import { useEffect, useState } from "react";
import { addressService } from "../../foundations/addressService";
import { AddressHomeView } from "../../../models/addresses/addressHomeView";
import { Address } from "../../../models/addresses/address";

type AddressHomeViewServiceResponse = {
    mappedAddresses: AddressHomeView[] | undefined;
    pages: Array<{ data: AddressHomeView[] }>;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: AddressHomeView[] }> } | undefined;
    refetch: () => void
}

export const AddressHomeViewService = {
    useGetAllAddresses: (searchTerm?: string): AddressHomeViewServiceResponse => {
        let query = ``;

        if (searchTerm) {
            const fields = [
                'uprn',
                'upsn',
                'organisationName',
                'departmentName',
                'subBuildingName',
                'buildingName',
                'buildingNumber',
                'dependentThoroughfare',
                'thoroughfare',
                'doubleDependentLocality',
                'dependentLocality',
                'postTown',
                'postCode'
            ];

            const filterConditions = fields
                .map(field => `contains(${field},'${searchTerm}')`)
                .join(' or ');

            query = query + `?filter=${filterConditions}`;
        }

        const response = addressService.useRetrieveAllAddressPages(query);

        const [mappedAddresses, setMappedAddresses] =
            useState<Array<AddressHomeView>>();

        const [pages, setPages] = useState<Array<{ data: AddressHomeView[] }>>([]);

        useEffect(() => {
            if (response.data && response.data.pages) {
                const addressArray: Array<AddressHomeView> = [];
                response.data.pages.forEach(x => {
                    x.data.forEach((address: Address) => {
                        addressArray.push(new AddressHomeView(
                            address.id,
                            address.isProcessing,
                            address.isSynced,
                            address.uprn,
                            address.upsn,
                            address.organisationName,
                            address.departmentName,
                            address.subBuildingName,
                            address.buildingName,
                            address.buildingNumber,
                            address.dependentThoroughfare,
                            address.thoroughfare,
                            address.doubleDependentLocality,
                            address.dependentLocality,
                            address.postTown,
                            address.postCode,
                            address.createdBy,
                            address.createdDate,
                            address.updatedBy,
                            address.updatedDate,
                        ));
                    });
                });

                setMappedAddresses(addressArray);
                setPages(response.data.pages);
            }
        }, [response.data]);

        return {
            mappedAddresses,
            pages,
            isLoading: response.isLoading,
            fetchNextPage: response.fetchNextPage,
            isFetchingNextPage: response.isFetchingNextPage,
            hasNextPage: !!response.hasNextPage,
            data: response.data,
            refetch: response.refetch
        };
    },
};
