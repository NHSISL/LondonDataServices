import { Guid } from 'guid-typescript';

export class SubscriberAgreementHomeView {
    public id: Guid;
    public supplierSharingAgreementShortName: Guid;;
    public createdDate?: Date;
    public createdBy?: string;
    public updatedDate?: Date;
    public updatedBy?: string;

    constructor(
        id: Guid,
        supplierSharingAgreementShortName: Guid,
        createdDate?: Date,
        createdBy?: string,
        updatedDate?: Date,
        updatedBy?: string,
    ) {
        this.id = id;
        this.supplierSharingAgreementShortName = supplierSharingAgreementShortName;
        this.createdDate = createdDate;
        this.createdBy = createdBy;
        this.updatedDate = updatedDate;
        this.updatedBy = updatedBy;
    }
}