"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Feature = void 0;
var featureDefinitions_1 = require("../../featureDefinitions");
var Feature = /** @class */ (function () {
    function Feature(feature) {
        this.feature = featureDefinitions_1.FeatureDefinitions[feature];
        if (this.feature === undefined) {
            throw new Error("Unknown feature - ensure appSettings and the feature definitions match.");
        }
    }
    return Feature;
}());
exports.Feature = Feature;
//# sourceMappingURL=feature.js.map