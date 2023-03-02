import { useEffect, useState } from "react";
import { IngestionTracking } from "../../models/ingestionTrackings/ingestionTracking";
import { IngestionTrackingHomeView } from "../../models/ingestionTrackings/ingestionTrackingHomeView";
import { ingestionTrackingService } from "../foundations/ingestionTrackingService";


export const ingestionTrackingHomeViewService = {

    useGetAllIngestionTrackings: (searchTerm?: string, source?: string) => {
        try {
            let query = `?$orderby=createdDate`;

            if (searchTerm) {
                query = query + `&$filter=contains(encryptedFileName,'${searchTerm}') or contains(decryptedFileName,'${searchTerm}')`;
            }

            if (source) {
                query = query + `&$filter=source eq '${source}'`;
            }

            const response = ingestionTrackingService.useGetAllIngestionTrackingPages(query);
            const [mappedIngestionTrackings, setMappedIngestionTrackings] = useState<Array<IngestionTrackingHomeView>>();
            const [pages, setPages] = useState<any>([]);

            useEffect(() => {
                if (response.data && response.data.pages) {
                    const IngestionTrackings: Array<IngestionTrackingHomeView> = [];
                    response.data.pages.forEach(x => {
                        x.data.forEach((ingestionTracking: IngestionTracking) => {
                            IngestionTrackings.push(new IngestionTrackingHomeView(
                                ingestionTracking.id,
                                ingestionTracking.source,
                                ingestionTracking.encryptedFileName,
                                ingestionTracking.decryptedFileName,
                                ingestionTracking.decrypted,
                                ingestionTracking.lastSeen,
                                ingestionTracking.fileDeleted,
                                ingestionTracking.recordCount,
                                ingestionTracking.encryptedFileSize,
                                ingestionTracking.decryptedFileSize,
                                ingestionTracking.audit,
                            ));
                        })
                        setMappedIngestionTrackings(IngestionTrackings);
                    });
                    setPages(response.data.pages);
                }
            }, [response.data]);

            return {
                mappedIngestionTrackings, pages, ...response
            }

        } catch (err) {
            throw err;
        }
    },
}