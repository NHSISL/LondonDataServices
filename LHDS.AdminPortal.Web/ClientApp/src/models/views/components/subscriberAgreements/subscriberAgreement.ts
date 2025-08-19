import { Guid } from 'guid-typescript';

export class SubscriberAgreementView {
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
    constructor(
        id: Guid,
        supplierSharingAgreementShortName?: string,
        supplierId?: string,
        ftpUserName?: string,
        ftpPublicKey?: string,
        gpgPublicKey?: string,
        isActive?: boolean,
        supplierSharingAgreementGuid?: Guid,
        lastPollStartDate?: Date,
        lastPollEndDate?: Date,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
    ) {
        this.id = id;
        this.supplierSharingAgreementShortName = supplierSharingAgreementShortName || "";
        this.supplierId = supplierId || "";
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
}