import { Guid } from 'guid-typescript';

export class PdsHomeView {
    public id: Guid;
    public correlationId: Guid;
    public message: string;
    public fileName: string;
    public messageId: string;
    public createdDate?: Date;
    public createdBy?: string;
    public updatedDate?: Date;
    public updatedBy?: string;

    constructor(
        id: Guid,
        correlationId: Guid,
        message: string,
        fileName: string,
        messageId: string,
        createdDate?: Date,
        createdBy?: string,
        updatedDate?: Date,
        updatedBy?: string,
    ) {
        this.id = id= id;
        this.correlationId = correlationId;
        this.message = message;
        this.fileName = fileName;
        this.messageId = messageId;
        this.createdDate = createdDate;
        this.createdBy = createdBy;
        this.updatedDate = updatedDate;
        this.updatedBy = updatedBy;
    }
}