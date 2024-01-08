import { Guid } from 'guid-typescript';

export class IngestionTrackingAudit {
    public id: Guid;
    public ingestionTrackingId: string;
    public message: string;
    public createdDate: Date;
    public createdBy: string;
    public updatedDate: Date;
    public updatedBy: string;

    constructor(ingestionTrackingAudit: any) {
        this.id = ingestionTrackingAudit.id ? Guid.parse(ingestionTrackingAudit.id) : Guid.parse(Guid.EMPTY);
        this.ingestionTrackingId = ingestionTrackingAudit.ingestionTrackingId;
        this.message = ingestionTrackingAudit.message || "";
        this.createdDate = ingestionTrackingAudit.createdDate ? new Date(ingestionTrackingAudit.createdDate) : new Date();
        this.createdBy = ingestionTrackingAudit.createdBy || "";
        this.updatedDate = ingestionTrackingAudit.updatedDate ? new Date(ingestionTrackingAudit.updatedDate) : new Date();
        this.updatedBy = ingestionTrackingAudit.updatedBy || "";
    }
}