import { Guid } from 'guid-typescript';

export class Supplier {
    public id: Guid;
    public name?: string;
    public friendlyName?: string;
    public description?: string;
    public isIngestionTracked?: boolean;
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
        this.isIngestionTracked = supplier.isIngestionTracked;
        this.canDecryptIngestionTracking = supplier.canDecryptIngestionTracking === true ? true : false;
        this.createdBy = supplier.createdBy;
        this.createdDate = new Date(supplier.createdDate);
        this.updatedBy = supplier.updatedBy;
        this.updatedDate = new Date(supplier.updatedDate);
    }
}