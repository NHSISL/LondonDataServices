import { Guid } from 'guid-typescript';
import { useEffect, useState } from 'react';
import { IngestionTracking } from '../../models/ingestionTrackings/ingestionTracking';
import { IngestionTrackingView } from '../../models/views/components/ingestionTracking/ingestionTrackingView';
import { ingestionTrackingService } from '../foundations/ingestionTrackingService';

export const ingestionTrackingViewService = {

    useGetAllIngestionTrackings: (searchTerm?: string) => {
        try {
            let query = `?$expand=supplier&$orderby=CreatedDate`;

            if (searchTerm) {
                query = query + `&$filter=contains(Id,'${searchTerm}')`;
            }

            const response = ingestionTrackingService.useGetAllIngestionTrackings(query);
            const [mappedIngestionTrackings, setMappedIngestionTrackings] = useState<Array<IngestionTrackingView>>();

            useEffect(() => {
                if (response.data) {
                    const ingestionTrackings =
                        response.data.map((ingestionTracking: IngestionTracking) => new IngestionTrackingView(
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
                            ingestionTracking.isDownloading,
                            ingestionTracking.isProcessing,
                            ingestionTracking.retryCount,
                            ingestionTracking.sourceFolderPath,
                            ingestionTracking.lastAttemptedDate,
                            ingestionTracking.dataSetSpecificationId,
                            ingestionTracking.batch,
                            ingestionTracking.objectName,
                            ingestionTracking.batchReadyFolderPath,
                            ingestionTracking.createdBy,
                            ingestionTracking.createdDate,
                            ingestionTracking.updatedBy,
                            ingestionTracking.updatedDate,
                            ingestionTracking.supplier,
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
            const query = `?$expand=supplier&$orderby=CreatedDate&$filter=id eq ${id}` ;

            const response = ingestionTrackingService.useGetAllIngestionTrackings(query);
            const [mappedIngestionTracking, setMappedIngestionTracking] = useState<IngestionTrackingView>();

            useEffect(() => {
                if (response.data) {
                    const ingestionTrackings =
                        response.data.map((ingestionTracking: IngestionTracking) => new IngestionTrackingView(
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
                            ingestionTracking.isDownloading,
                            ingestionTracking.isProcessing,
                            ingestionTracking.retryCount,
                            ingestionTracking.sourceFolderPath,
                            ingestionTracking.lastAttemptedDate,
                            ingestionTracking.dataSetSpecificationId,
                            ingestionTracking.batch,
                            ingestionTracking.objectName,
                            ingestionTracking.batchReadyFolderPath,
                            ingestionTracking.createdBy,
                            ingestionTracking.createdDate,
                            ingestionTracking.updatedBy,
                            ingestionTracking.updatedDate,
                            ingestionTracking.supplier,
                        ));

                    setMappedIngestionTracking(ingestionTrackings[0]);
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