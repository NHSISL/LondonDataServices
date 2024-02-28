"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.featureService = void 0;
var react_query_1 = require("react-query");
var apiBroker_features_1 = require("../../brokers/apiBroker.features");
exports.featureService = {
    useGetAllFeatures: function () {
        var featureBroker = new apiBroker_features_1.default();
        return (0, react_query_1.useQuery)(["FeaturesGetAll"], function () { return featureBroker.GetAllFeatureAsync(); }, { staleTime: Infinity });
    }
};
//# sourceMappingURL=featureService.js.map