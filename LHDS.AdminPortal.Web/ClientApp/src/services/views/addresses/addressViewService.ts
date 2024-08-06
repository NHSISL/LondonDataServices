import { Guid } from "guid-typescript";
import { useEffect, useState } from "react";
import { addressService } from "../../foundations/addressService";
import { Address } from "../../../models/addresses/address";
import { AddressView } from "../../../models/views/components/addresses/addressView";

export const addressViewService = {

    useCreateAddress: () => {
        return addressService.useCreateAddress();
    },

    useGetAllAddresses: (searchTerm?: string) => {
        try {
            let query = '?$orderby=name';

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

                query = query + `&$filter=${filterConditions}`;
            }

            const response = addressService.useRetrieveAllAddresses(query);
            const [mappedAddresses, setMappedAddresses] = useState<Array<AddressView>>([]);

            useEffect(() => {
                if (response.data) {
                    const addresses = response.data.map((address: Address) =>
                        new AddressView(
                            addresses.id,
                            addresses.isProcessing,
                            addresses.isSynced,
                            addresses.uprn,
                            addresses.upsn,
                            addresses.organisationName,
                            addresses.departmentName,
                            addresses.subBuildingName,
                            addresses.buildingName,
                            addresses.buildingNumber,
                            addresses.dependentThoroughfare,
                            addresses.thoroughfare,
                            addresses.doubleDependentLocality,
                            addresses.dependentLocality,
                            addresses.postTown,
                            addresses.postCode,
                            addresses.createdBy,
                            addresses.createdDate,
                            addresses.updatedBy,
                            addresses.updatedDate,
                        ));

                    setMappedAddresses(addresses);
                }
            }, [response.data]);

            return {
                mappedAddresses, ...response
            }
        } catch (err) {
            throw err;
        }
    },

    useGetAddressById: (id: Guid) => {
        try {
            const query = `?$filter=id eq ${id}`
            const response = addressService.useRetrieveAllAddresses(query);
            const [mappedAddress, setMappedAddress] = useState<AddressView>();

            useEffect(() => {
                if (response.data && response.data[0]) {
                    const address = new AddressView(
                        response.data[0].id,
                        response.data[0].isProcessing,
                        response.data[0].isSynced,
                        response.data[0].uprn,
                        response.data[0].upsn,
                        response.data[0].organisationName,
                        response.data[0].departmentName,
                        response.data[0].subBuildingName,
                        response.data[0].buildingName,
                        response.data[0].buildingNumber,
                        response.data[0].dependentThoroughfare,
                        response.data[0].thoroughfare,
                        response.data[0].doubleDependentLocality,
                        response.data[0].dependentLocality,
                        response.data[0].postTown,
                        response.data[0].postCode,
                        response.data[0].createdBy,
                        response.data[0].createdDate,
                        response.data[0].updatedBy,
                        response.data[0].updatedDate
                    );

                    setMappedAddress(address);
                }
            }, [response.data]);

            return {
                mappedAddress, ...response
            }
        } catch (err) {
            throw err;
        }
    },

    useUpdateAddress: () => {
        return addressService.useModifyAddress();
    },

    useRemoveAddress: () => {
        return addressService.useRemoveAddress();
    },
}