"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SubscriberCredentialView = void 0;
var SubscriberCredentialView = /** @class */ (function () {
    function SubscriberCredentialView(id, supplierSharingAgreementShortName, ftpUserName, ftpPublicKey, gpgPublicKey, isActive, supplierSharingAgreementGuid, lastPollStartDate, lastPollEndDate, createdBy, createdDate, updatedBy, updatedDate) {
        this.id = id;
        this.supplierSharingAgreementShortName = supplierSharingAgreementShortName || "";
        this.ftpUserName = ftpUserName || "";
        this.ftpPublicKey = ftpPublicKey || "";
        this.gpgPublicKey = gpgPublicKey || "";
        this.isActive = isActive === true ? true : false;
        this.supplierSharingAgreementGuid = supplierSharingAgreementGuid;
        this.lastPollStartDate = lastPollStartDate !== undefined ? new Date(lastPollStartDate) : undefined;
        this.lastPollEndDate = lastPollEndDate !== undefined ? new Date(lastPollEndDate) : undefined;
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate;
        this.updatedBy = updatedBy !== undefined ? updatedBy : '';
        this.updatedDate = updatedDate;
    }
    return SubscriberCredentialView;
}());
exports.SubscriberCredentialView = SubscriberCredentialView;
//# sourceMappingURL=subscriberCredentialView.js.map