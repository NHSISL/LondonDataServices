import { Guid } from 'guid-typescript';

export class Supplier {
    public id: Guid;
    public name: string;
    public friendlyName: string;
    public description: string;
    public landingManualTriggerUrl: string;
    public createdDate: Date;
    public updatedBy: string;
    public updatedDate: Date;

    constructor(supplier: any) {
        this.id = supplier.id ? Guid.parse(supplier.id) : Guid.parse(Guid.EMPTY);
        this.name = supplier.message || "";
        this.friendlyName = supplier.message || "";
        this.description = supplier.message || "";
        this.landingManualTriggerUrl = supplier.message || "";
        this.createdDate = supplier.createdDate ? new Date(supplier.createdDate) : new Date();
        this.updatedBy = supplier.updatedBy || "";
        this.updatedDate = supplier.updatedDate ? new Date(supplier.updatedDate) : new Date();
    }
}