import { Feature } from "../models/features/feature";
import ApiBroker from "./apiBroker";

class FeatureBroker {
    relativeAuditUrl = '/api/features';

    private apiBroker: ApiBroker = new ApiBroker();

    async GetAllFeatureAsync() {
        var url = this.relativeAuditUrl;

        return await this.apiBroker.GetAsync(url)
            .then(result => {
                return result.data.map((feature: any) => new Feature(feature))
            }
            );
    }
}
export default FeatureBroker;