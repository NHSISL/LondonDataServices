import { Guid } from 'guid-typescript';

export class Pds {
    public id: Guid;
    public correlationId: Guid;
    public message: string;
    public fileName: string;
    public messageId: string;
    public createdDate?: Date;
    public createdBy?: string;
    public updatedDate?: Date;
    public updatedBy?: string;

    constructor(pds: any) {
        this.id = pds.id ? Guid.parse(pds.id) : Guid.parse(Guid.EMPTY);
        this.correlationId = pds.correlationId;
        this.message = pds.message;
        this.fileName = pds.fileName;
        this.messageId = pds.messageId;
        this.createdDate = pds.createdDate ? new Date(pds.createdDate) : new Date();
        this.createdBy = pds.createdBy || "";
        this.updatedDate = pds.updatedDate ? new Date(pds.updatedDate) : new Date();
        this.updatedBy = pds.updatedBy || "";
    }
}