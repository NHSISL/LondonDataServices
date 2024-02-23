"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SubscriberAgreement = void 0;
var guid_typescript_1 = require("guid-typescript");
var SubscriberAgreement = /** @class */ (function () {
    function SubscriberAgreement(SubscriberAgreement) {
        this.id = SubscriberAgreement.id ? guid_typescript_1.Guid.parse(SubscriberAgreement.id) : guid_typescript_1.Guid.parse(guid_typescript_1.Guid.EMPTY);
        this.supplierSharingAgreementShortName = SubscriberAgreement.supplierSharingAgreementShortName || "";
        this.ftpUserName = SubscriberAgreement.ftpUserName || "";
        this.ftpPublicKey = SubscriberAgreement.ftpPublicKey || "";
        this.gpgPublicKey = SubscriberAgreement.gpgPublicKey || "";
        this.isActive = SubscriberAgreement.iIsActive === true ? true : false;
        this.supplierSharingAgreementGuid =
            SubscriberAgreement.supplierSharingAgreementGuid
                ? guid_typescript_1.Guid.parse(SubscriberAgreement.supplierSharingAgreementGuid)
                : guid_typescript_1.Guid.parse(guid_typescript_1.Guid.EMPTY);
        this.lastPollStartDate = SubscriberAgreement.lastPollStartDate ? new Date(SubscriberAgreement.lastPollStartDate) : undefined;
        this.lastPollEndDate = SubscriberAgreement.lastPollEndDate ? new Date(SubscriberAgreement.lastPollEndDate) : undefined;
        this.createdDate = SubscriberAgreement.createdDate;
        this.createdBy = SubscriberAgreement.createdBy;
        this.createdDate = new Date(SubscriberAgreement.createdDate);
        this.updatedBy = SubscriberAgreement.updatedBy;
        this.updatedDate = new Date(SubscriberAgreement.updatedDate);
    }
    return SubscriberAgreement;
}());
exports.SubscriberAgreement = SubscriberAgreement;
//# sourceMappingURL=subscriberAgreements.js.map