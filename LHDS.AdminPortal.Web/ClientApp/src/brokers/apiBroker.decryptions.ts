import ApiBroker from "./apiBroker";

class DecryptionBroker {
    relativeAuditUrl = '/api/decryptions';

    private apiBroker: ApiBroker = new ApiBroker();

    async GetDocumentByFileNameToDecryptAsync(fileName: string) {
        var url = this.relativeAuditUrl + '/' + encodeURIComponent(fileName);

        return await this.apiBroker.GetAsync(url);
    }
}
export default DecryptionBroker;