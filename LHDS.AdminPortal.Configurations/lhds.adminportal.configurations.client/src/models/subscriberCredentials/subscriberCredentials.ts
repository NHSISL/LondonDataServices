export class SubscriberCredential {
    public id: string;
    public supplierSharingAgreementShortName: string;
    public ftpUserName?: string;
    public ftpPublicKey?: string;
    public gpgPublicKey?: string;
    public isActive: boolean;
    public supplierSharingAgreementGuid?: string;
    public lastPollStartDate?: Date;
    public lastPollEndDate?: Date;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(SubscriberCredential: SubscriberCredential) {
        this.id = SubscriberCredential.id ? SubscriberCredential.id : "";
        this.supplierSharingAgreementShortName = SubscriberCredential.supplierSharingAgreementShortName ;
        this.ftpUserName = SubscriberCredential.ftpUserName ;
        this.ftpPublicKey = SubscriberCredential.ftpPublicKey;
        this.gpgPublicKey = SubscriberCredential.gpgPublicKey;
        this.isActive = SubscriberCredential.isActive === true ? true : false;

        this.supplierSharingAgreementGuid =
            SubscriberCredential.supplierSharingAgreementGuid
            ? SubscriberCredential.supplierSharingAgreementGuid
                : "";

        this.lastPollStartDate = SubscriberCredential.lastPollStartDate;
        this.lastPollEndDate = SubscriberCredential.lastPollEndDate;
        this.createdDate = SubscriberCredential.createdDate;
        this.createdBy = SubscriberCredential.createdBy;
        this.createdDate = new Date(SubscriberCredential.createdDate);
        this.updatedBy = SubscriberCredential.updatedBy;
        this.updatedDate = new Date(SubscriberCredential.updatedDate);
    }
}