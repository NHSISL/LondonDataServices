import { Guid } from 'guid-typescript';

export class Supplier {
    public id: Guid;
    public name?: string;
    public friendlyName?: string;
    public description?: string;
    public landingManualTriggerUrl?: string;
    public decryptionManualTriggerUrl?: string;
    public canDecryptIngestionTracking?: boolean;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(supplier: any) {
        this.id = supplier.id ? Guid.parse(supplier.id) : Guid.parse(Guid.EMPTY);
        this.name = supplier.name;
        this.friendlyName = supplier.friendlyName;
        this.description = supplier.description;
        this.landingManualTriggerUrl = supplier.landingManualTriggerUrl;
        this.decryptionManualTriggerUrl = supplier.decryptionManualTriggerUrl;
        this.canDecryptIngestionTracking = supplier.canDecryptIngestionTracking === true ? true : false;
        this.createdBy = supplier.createdBy;
        this.createdDate = new Date(supplier.createdDate);
        this.updatedBy = supplier.updatedBy;
        this.updatedDate = new Date(supplier.updatedDate);
    }
}