import { Guid } from 'guid-typescript';

export class IngestionTrackingAuditView {
    public id: Guid;
    public ingestionTrackingId: string;
    public message: string;
    public createdDate?: Date;

    constructor(
        id: Guid,
        ingestionTrackingId: string,
        message: string,
        createdDate: Date) 
    {
        this.id = id ;
        this.ingestionTrackingId = ingestionTrackingId;
        this.message = message || "";
        this.createdDate = createdDate;
    }
}