import { Document } from "../models/documents/document";
import ApiBroker from "./apiBroker";

class DocumentBroker {
    relativeDocumentUrl = '/api/documents';

    private apiBroker: ApiBroker = new ApiBroker();

    async GetDownloadLinkAsync(fileName: string) {
        const url = `${this.relativeDocumentUrl}/${encodeURIComponent(fileName)}`;

        return await this.apiBroker.GetAsync(url)
            .then(result => new Document(result.data));
    }
}
export default DocumentBroker;