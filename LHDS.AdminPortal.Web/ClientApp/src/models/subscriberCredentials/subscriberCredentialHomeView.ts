import { Guid } from 'guid-typescript';

export class SubscriberCredentialHomeView {
    public id: Guid;
    public supplierSharingAgreementShortName: string;
    public supplierSharingAgreementGuid: Guid;
    public createdDate?: Date;
    public createdBy?: string;
    public updatedDate?: Date;
    public updatedBy?: string;

    constructor(
        id: Guid,
        supplierSharingAgreementShortName: string,
        supplierSharingAgreementGuid: Guid,
        createdDate?: Date,
        createdBy?: string,
        updatedDate?: Date,
        updatedBy?: string,
    ) {
        this.id = id;
        this.supplierSharingAgreementShortName = supplierSharingAgreementShortName;
        this.supplierSharingAgreementGuid = supplierSharingAgreementGuid;
        this.createdDate = createdDate;
        this.createdBy = createdBy;
        this.updatedDate = updatedDate;
        this.updatedBy = updatedBy;
    }
}