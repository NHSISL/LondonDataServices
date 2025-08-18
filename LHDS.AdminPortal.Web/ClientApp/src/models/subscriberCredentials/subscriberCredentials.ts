import { Guid } from 'guid-typescript';

export class SubscriberCredential {
    public id: Guid;
    public supplierSharingAgreementShortName: string;
    public supplierId: string;
    public ftpUserName?: string;
    public ftpPublicKey?: string;
    public gpgPublicKey?: string;
    public isActive: boolean;
    public supplierSharingAgreementGuid?: Guid;
    public lastPollStartDate?: Date;
    public lastPollEndDate?: Date;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(SubscriberCredential: any) {
        this.id = SubscriberCredential.id ? Guid.parse(SubscriberCredential.id) : Guid.parse(Guid.EMPTY);
        this.supplierSharingAgreementShortName = SubscriberCredential.supplierSharingAgreementShortName;
        this.supplierId = SubscriberCredential.supplierId;
        this.ftpUserName = SubscriberCredential.ftpUserName ;
        this.ftpPublicKey = SubscriberCredential.ftpPublicKey;
        this.gpgPublicKey = SubscriberCredential.gpgPublicKey;
        this.isActive = SubscriberCredential.isActive === true ? true : false;

        this.supplierSharingAgreementGuid =
            SubscriberCredential.supplierSharingAgreementGuid
            ? Guid.parse(SubscriberCredential.supplierSharingAgreementGuid)
                : Guid.parse(Guid.EMPTY);

        this.lastPollStartDate = SubscriberCredential.lastPollStartDate;
        this.lastPollEndDate = SubscriberCredential.lastPollEndDate;
        this.createdDate = SubscriberCredential.createdDate;
        this.createdBy = SubscriberCredential.createdBy;
        this.createdDate = new Date(SubscriberCredential.createdDate);
        this.updatedBy = SubscriberCredential.updatedBy;
        this.updatedDate = new Date(SubscriberCredential.updatedDate);
    }
}