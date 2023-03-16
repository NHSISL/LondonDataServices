import { useEffect, useState } from 'react';
import { DocumentView } from '../../../models/views/components/documents/documentView';
import { documentService } from '../../foundations/documentService';

export const documentViewService = {

    useGetDownloadLinkByFileName: (fileName: string) => {
        try {
            const response = documentService.useGetDownloadLinkByFileName(fileName);
            const [mappedDocument, setMappedDocument] = useState<DocumentView>();

            useEffect(() => {
                if (response.data) {
                    const document = new DocumentView(
                        response.data.Url
                    );

                    setMappedDocument(document);
                }
            }, [response.data]);

            return {
                mappedDocument, ...response
            }
        } catch (err) {
            throw err;
        }
    },
}