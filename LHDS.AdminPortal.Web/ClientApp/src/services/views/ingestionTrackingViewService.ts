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
                        ingestionTracking.createdDate,
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
    },

    useGetIngestionTrackingById: (id: string) => {
        try {
            const query = `?$expand=audit&$filter=id eq ${id}`
            const response = ingestionTrackingService.useGetAllIngestionTrackings(query);
            const [mappedIngestionTracking, setMappedIngestionTracking] = useState<IngestionTrackingView>();

            useEffect(() => {
                if (response.data && response.data[0]) {
                    const ingestionTracking = new IngestionTrackingView(
                        response.data[0].id,
                        response.data[0].source,
                        response.data[0].encryptedFileName,
                        response.data[0].decryptedFileName,
                        response.data[0].decrypted,
                        response.data[0].createdDate,
                        response.data[0].lastSeen,
                        response.data[0].fileDeleted,
                        response.data[0].recordCount,
                        response.data[0].encryptedFileSize,
                        response.data[0].decryptedFileSize);

                    setMappedIngestionTracking(ingestionTracking);
                }
            }, [response.data]);

            return {
                mappedIngestionTracking, ...response
            }
        } catch (err) {
            throw err;
        }
    },
}