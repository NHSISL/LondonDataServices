import { Guid } from 'guid-typescript';

export class SubscriberAgreement {
    public id: Guid;
    public SupplierSharingAgreementShortName: string;
    public FtpUserName: string;
    public FtpPublicKey: string;
    public GpgPublicKey: string;
    public IsActive: boolean;
    public SupplierSharingAgreementGuid?: Guid;
    public LastPollStartDate?: Date;
    public LastPollEndDate?: Date;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(SubscriberAgreement: any) {
        this.id = SubscriberAgreement.id ? Guid.parse(SubscriberAgreement.id) : Guid.parse(Guid.EMPTY);
        this.SupplierSharingAgreementShortName = SubscriberAgreement.SupplierSharingAgreementShortName || "";
        this.FtpUserName = SubscriberAgreement.FtpUserName || "";
        this.FtpPublicKey = SubscriberAgreement.FtpPublicKey || "";
        this.GpgPublicKey = SubscriberAgreement.GpgPublicKey || "";
        this.IsActive = SubscriberAgreement.IsActive === true ? true : false;

        this.SupplierSharingAgreementGuid =
            SubscriberAgreement.SupplierSharingAgreementGuid
                ? Guid.parse(SubscriberAgreement.SupplierSharingAgreementGuid)
                : Guid.parse(Guid.EMPTY);

        this.LastPollStartDate = SubscriberAgreement.LastPollStartDate;
        this.LastPollEndDate = SubscriberAgreement.LastPollEndDate;
        this.createdDate = SubscriberAgreement.createdDate;
        this.createdBy = SubscriberAgreement.createdBy;
        this.createdDate = new Date(SubscriberAgreement.createdDate);
        this.updatedBy = SubscriberAgreement.updatedBy;
        this.updatedDate = new Date(SubscriberAgreement.updatedDate);
    }
}