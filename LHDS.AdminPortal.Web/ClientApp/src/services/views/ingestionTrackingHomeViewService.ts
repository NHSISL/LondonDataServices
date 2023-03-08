import { useEffect, useState } from "react";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import { IngestionTrackingHomeView } from "../../models/ingestionTrackings/ingestionTrackingHomeView";
import { LookupView } from "../../models/views/components/lookups/lookupView";
import { ingestionTrackingService } from "../foundations/ingestionTrackingService";
import { lookupViewService } from "./lookups/lookupViewService";

type IngestionTrackingHomeViewServiceResponse = {
    mappedIngestionTrackings: IngestionTrackingHomeView[] | undefined;
    pages: any;
    supplierOptions: Array<LookupView>; // Include the supplierOptions array in the returned object
    selectedOption: string | undefined;
    handleSupplierChange: (event: React.ChangeEvent<{ value: unknown }>) => void;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: any;
}

export const ingestionTrackingHomeViewService = {
    useGetAllIngestionTrackings: (searchTerm?: string, selectedSupplier?: string): IngestionTrackingHomeViewServiceResponse => {
        try {
            let query = `?$orderby=createdDate`;

            if (selectedSupplier && searchTerm) {
                query = query + `&$filter=supplierId eq '${selectedSupplier}' and (contains(encryptedFileName,'${searchTerm}') or contains(decryptedFileName,'${searchTerm}'))`;
            } else if (selectedSupplier) {
                query = query + `&$filter=supplierId eq '${selectedSupplier}'`;
            } else if (searchTerm) {
                query = query + `&$filter=contains(encryptedFileName,'${searchTerm}') or contains(decryptedFileName,'${searchTerm}')`;
            }

            const response = ingestionTrackingService.useGetAllIngestionTrackingPages(query);
            const [mappedIngestionTrackings, setMappedIngestionTrackings] = useState<Array<IngestionTrackingHomeView>>();
            const [pages, setPages] = useState<any>([]);
            const [supplierOptions, setSupplierOptions] = useState<Array<LookupView>>([]);
            const [selectedOption, setSelectedOption] = useState<string>();

            useEffect(() => {
                // Fetch the supplier list
                const fetchSupplierList = async () => {
                    try {
                        const suppliers = await lookupViewService.useGetSupplierList();
                        const supplierLookup: LookupView[] = suppliers?.data?.map((supplier: any) => ({
                            id: supplier.id,
                            name: supplier.name
                        })) || [];
                        setSupplierOptions([{ id: '', name: 'Please select...' }, ...supplierLookup]);
                    } catch (err) {
                        console.log('Error fetching supplier list', err);
                    }
                }

                if (response.data && response.data.pages) {
                    const ingestionTrackings: Array<IngestionTrackingHomeView> = [];
                    response.data.pages.forEach(x => {
                        x.data.forEach((ingestionTracking: IngestionTracking) => {
                            ingestionTrackings.push(new IngestionTrackingHomeView(
                                ingestionTracking.id,
                                ingestionTracking.fileName,
                                ingestionTracking.supplierId,
                                ingestionTracking.encryptedFileName,
                                ingestionTracking.decryptedFileName,
                                ingestionTracking.decrypted,
                                ingestionTracking.lastSeen,
                                ingestionTracking.fileDeleted,
                                ingestionTracking.recordCount,
                                ingestionTracking.encryptedFileSize,
                                ingestionTracking.decryptedFileSize,
                                ingestionTracking.audit
                            ));
                        });
                    });
                    setMappedIngestionTrackings(ingestionTrackings);
                    setPages(response.data.pages);
                    if (selectedSupplier) {
                        setMappedIngestionTrackings(ingestionTrackings.filter(x => x.supplierId === selectedSupplier));
                    }
                }
            }, [response.data, setSupplierOptions, selectedSupplier]);



            const handleSupplierChange = (event: React.ChangeEvent<{ value: unknown }>) => {
                setSelectedOption(event.target.value as string);
            };

            return {
                mappedIngestionTrackings,
                pages,
                supplierOptions,
                selectedOption,
                handleSupplierChange,
                isLoading: response.isLoading,
                fetchNextPage: response.fetchNextPage,
                isFetchingNextPage: response.isFetchingNextPage,
                hasNextPage: !!response.hasNextPage,
                data: response.data,
            };
        } catch (err) {
            throw err;
        }
    },
};
