import { documentService } from '../../foundations/documentService';

export const documentViewService = {

    useGetDownloadLinkByFileName: (fileName: string) => {
        return documentService.useGetDownloadLinkByFileName();
    },
}