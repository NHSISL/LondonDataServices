"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SubscriberCredential = void 0;
var guid_typescript_1 = require("guid-typescript");
var SubscriberCredential = /** @class */ (function () {
    function SubscriberCredential(SubscriberCredential) {
        this.id = SubscriberCredential.id ? guid_typescript_1.Guid.parse(SubscriberCredential.id) : guid_typescript_1.Guid.parse(guid_typescript_1.Guid.EMPTY);
        this.supplierSharingAgreementShortName = SubscriberCredential.supplierSharingAgreementShortName;
        this.ftpUserName = SubscriberCredential.ftpUserName;
        this.ftpPublicKey = SubscriberCredential.ftpPublicKey;
        this.gpgPublicKey = SubscriberCredential.gpgPublicKey;
        this.isActive = SubscriberCredential.isActive === true ? true : false;
        this.supplierSharingAgreementGuid =
            SubscriberCredential.supplierSharingAgreementGuid
                ? guid_typescript_1.Guid.parse(SubscriberCredential.supplierSharingAgreementGuid)
                : guid_typescript_1.Guid.parse(guid_typescript_1.Guid.EMPTY);
        this.lastPollStartDate = SubscriberCredential.lastPollStartDate;
        this.lastPollEndDate = SubscriberCredential.lastPollEndDate;
        this.createdDate = SubscriberCredential.createdDate;
        this.createdBy = SubscriberCredential.createdBy;
        this.createdDate = new Date(SubscriberCredential.createdDate);
        this.updatedBy = SubscriberCredential.updatedBy;
        this.updatedDate = new Date(SubscriberCredential.updatedDate);
    }
    return SubscriberCredential;
}());
exports.SubscriberCredential = SubscriberCredential;
//# sourceMappingURL=subscriberCredentials.js.map