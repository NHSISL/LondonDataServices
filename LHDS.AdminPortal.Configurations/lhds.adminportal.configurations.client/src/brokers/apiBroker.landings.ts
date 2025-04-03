import ApiBroker from "./apiBroker";

class LandingBroker {
    relativeAuditUrl = '/api/landings';

    private apiBroker: ApiBroker = new ApiBroker();

    async GetLandingDocumentByFileNameAsync(fileName: string) {
        var url = this.relativeAuditUrl + '/' + encodeURIComponent(fileName);

        return await this.apiBroker.GetAsync(url);
    }
}
export default LandingBroker;