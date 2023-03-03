import { Guid } from 'guid-typescript';

export class Audit {
    public id: Guid;
    public ingestionTrackingId: string;
    public message: string;
    public createdDate: Date;
    public createdBy: string;
    public updatedDate: Date;
    public updatedBy: string;

    constructor(audit: any) {
        this.id = audit.id ? Guid.parse(audit.id) : Guid.parse(Guid.EMPTY);
        this.ingestionTrackingId = audit.ingestionTrackingId;
        this.message = audit.message || "";
        this.createdDate = audit.createdDate ? new Date(audit.createdDate) : new Date();
        this.createdBy = audit.createdBy || "";
        this.updatedDate = audit.updatedDate ? new Date(audit.updatedDate) : new Date();
        this.updatedBy = audit.updatedBy || "";
    }
}