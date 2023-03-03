import { useEffect, useState } from 'react';
import { IngestionTracking } from '../../models/ingestionTrackings/ingestionTracking';
import { IngestionTrackingView } from '../../models/views/components/ingestionTracking/ingestionTrackingView';
import { ingestionTrackingService } from '../foundations/ingestionTrackingService';

export const ingestionTrackingViewService = {

    useGetAllIngestionTrackings: (searchTerm?: string) => {
        try {
            let query = `?$expand=audit&$orderby=createdDate`;

            if (searchTerm) {
                query = query + `&$filter=contains(Id,'${searchTerm}')`;
            }

            const response = ingestionTrackingService.useGetAllIngestionTrackings(query);
            const [mappedIngestionTrackings, setMappedIngestionTrackings] = useState<Array<IngestionTrackingView>>();

            useEffect(() => {
                if (response.data) {
                    const ingestionTrackings = response.data.map((ingestionTracking: IngestionTracking) => new IngestionTrackingView(
                        ingestionTracking.id,
                        ingestionTracking.source,
                        ingestionTracking.encryptedFileName,
                        ingestionTracking.decryptedFileName,
                        ingestionTracking.decrypted,
                        ingestionTracking.lastSeen,
                        ingestionTracking.fileDeleted,
                        ingestionTracking.recordCount,
                        ingestionTracking.encryptedFileSize,
                        ingestionTracking.decryptedFileSize
                    ));

                    setMappedIngestionTrackings(ingestionTrackings);
                }
            }, [response.data]);

            return {
                mappedIngestionTrackings, ...response
            }
        } catch (err) {
            throw err;
        }
    }
}