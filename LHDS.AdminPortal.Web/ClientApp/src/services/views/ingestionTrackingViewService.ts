import { Guid } from 'guid-typescript';
import { useEffect, useState } from 'react';
import { IngestionTracking } from '../../models/ingestionTrackings/ingestionTracking';
import { IngestionTrackingView } from '../../models/views/components/ingestionTracking/ingestionTrackingView';
import { ingestionTrackingService } from '../foundations/ingestionTrackingService';

export const ingestionTrackingViewService = {

    useGetAllIngestionTrackings: (searchTerm?: string) => {
        try {
            let query = `?$expand=audit&$expand=supplier&$orderby=createdDate`;

            if (searchTerm) {
                query = query + `&$filter=contains(Id,'${searchTerm}')`;
            }

            const response = ingestionTrackingService.useGetAllIngestionTrackings(query);
            const [mappedIngestionTrackings, setMappedIngestionTrackings] = useState<Array<IngestionTrackingView>>();

            useEffect(() => {
                if (response.data) {
                    const ingestionTrackings = response.data.map((ingestionTracking: IngestionTracking) => new IngestionTrackingView(
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

    useGetIngestionTrackingById: (id: Guid) => {
        try {
            const response = ingestionTrackingService.useGetIngestionTrackingById(id);
            const [mappedIngestionTracking, setMappedIngestionTracking] = useState<IngestionTrackingView>();

            useEffect(() => {
                if (response.data) {
                    const ingestionTracking = new IngestionTrackingView(
                        response.data.id,
                        response.data.fileName,
                        response.data.supplierId,
                        response.data.encryptedFileName,
                        response.data.decryptedFileName,
                        response.data.decrypted,
                        response.data.lastSeen,
                        response.data.fileDeleted,
                        response.data.recordCount,
                        response.data.encryptedFileSize,
                        response.data.decryptedFileSize,
                        response.data.createdBy,
                        response.data.createdDate,
                        response.data.updatedBy,
                        response.data.updatedDate,
                    );

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