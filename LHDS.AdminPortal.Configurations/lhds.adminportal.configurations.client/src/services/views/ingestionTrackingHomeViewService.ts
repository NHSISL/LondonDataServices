import { useEffect, useState } from "react";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import { IngestionTrackingHomeView } from "../../models/ingestionTrackings/ingestionTrackingHomeView";
import { LookupView } from "../../models/views/components/lookups/lookupView";
import { ingestionTrackingService } from "../foundations/ingestionTrackingService";
import { landingService } from "../foundations/landingService";
import { decryptionService } from "../foundations/decryptionService";
import { documentService } from "../foundations/documentService";

type IngestionTrackingHomeViewServiceResponse = {
    mappedIngestionTrackings: IngestionTrackingHomeView[] | undefined;
    pages: Array<{ data: IngestionTrackingHomeView[] }>;
    supplierOptions: Array<LookupView>; // Include the supplierOptions array in the returned object
    selectedOption: string | undefined;
    handleSupplierChange: (event: React.ChangeEvent<{ value: unknown }>) => void;
    isLoading: boolean;
    fetchNextPage: () => void;
    isFetchingNextPage: boolean;
    hasNextPage: boolean;
    data: { pages: Array<{ data: IngestionTrackingHomeView[] }> } | undefined;
    refetch: () => void
}

export const ingestionTrackingHomeViewService = {
    useGetAllIngestionTrackings: (searchTerm?: string, supplierId?: string): IngestionTrackingHomeViewServiceResponse => {

        let query = `?$orderby=CreatedDate desc&$expand=supplier`;

        if (searchTerm) {
            query = query + `&$filter=contains(fileName,'${searchTerm}') or contains(decryptedFileName,'${searchTerm}')`;
        }

        if (supplierId) {
            query = query + `&$filter=supplier/id eq ${supplierId}`;
        }

        const response = ingestionTrackingService.useGetAllIngestionTrackingPages(query);
        const [mappedIngestionTrackings, setMappedIngestionTrackings] = useState<Array<IngestionTrackingHomeView>>();
        const [pages, setPages] = useState<Array<{ data: IngestionTrackingHomeView[] }>>([]);
        const [supplierOptions, setSupplierOptions] = useState<Array<LookupView>>([]);
        const [selectedOption, setSelectedOption] = useState<string>();

        useEffect(() => {
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
                            ingestionTracking.createdBy,
                            ingestionTracking.createdDate,
                            ingestionTracking.updatedBy,
                            ingestionTracking.updatedDate,
                            ingestionTracking.supplier,
                        ));
                    });
                });

                setMappedIngestionTrackings(ingestionTrackings);
                setPages(response.data.pages);
            }
        }, [response.data, setSupplierOptions]);

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
            refetch: response.refetch
        };
    },

    useRelandIngestionTracking: () => {
        return landingService.useGetDownloadLinkByFileName();
    },

    useRedecryptIngestionTracking: () => {
        return decryptionService.useGetDocumentByFileNameToDecryptAsync();
    },

    useDownloadEncryptedDocument: () => {
        return documentService.useGetDownloadLinkByFileName();
    },

    useDownloadDecryptedDocument: () => {
        return documentService.useGetDownloadLinkByFileName();
    },
};
