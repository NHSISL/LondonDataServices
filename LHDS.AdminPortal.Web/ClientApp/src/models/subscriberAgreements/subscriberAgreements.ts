import { Guid } from 'guid-typescript';

export class SubscriberAgreement {
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

    constructor(SubscriberAgreement: any) {
        this.id = SubscriberAgreement.id ? Guid.parse(SubscriberAgreement.id) : Guid.parse(Guid.EMPTY);
        this.supplierSharingAgreementShortName = SubscriberAgreement.supplierSharingAgreementShortName || "";
        this.supplierId = SubscriberAgreement.supplierId || "";
        this.ftpUserName = SubscriberAgreement.ftpUserName || "";
        this.ftpPublicKey = SubscriberAgreement.ftpPublicKey || "";
        this.gpgPublicKey = SubscriberAgreement.gpgPublicKey || "";
        this.isActive = SubscriberAgreement.isActive === true ? true : false;

        this.supplierSharingAgreementGuid =
            SubscriberAgreement.supplierSharingAgreementGuid
                ? Guid.parse(SubscriberAgreement.supplierSharingAgreementGuid)
                : Guid.parse(Guid.EMPTY);

        this.lastPollStartDate = SubscriberAgreement.lastPollStartDate ? new Date(SubscriberAgreement.lastPollStartDate) : undefined;
        this.lastPollEndDate = SubscriberAgreement.lastPollEndDate ? new Date(SubscriberAgreement.lastPollEndDate) : undefined;
        this.createdDate = SubscriberAgreement.createdDate;
        this.createdBy = SubscriberAgreement.createdBy;
        this.createdDate = new Date(SubscriberAgreement.createdDate);
        this.updatedBy = SubscriberAgreement.updatedBy;
        this.updatedDate = new Date(SubscriberAgreement.updatedDate);
    }
}