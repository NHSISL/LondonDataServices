export class PdsHomeView {
    public id: string;
    public correlationId: string;
    public message: string;
    public fileName: string;
    public messageId: string;
    public createdDate?: Date;
    public createdBy?: string;
    public updatedDate?: Date;
    public updatedBy?: string;

    constructor(
        id: string,
        correlationId: string,
        message: string,
        fileName: string,
        messageId: string,
        createdDate?: Date,
        createdBy?: string,
        updatedDate?: Date,
        updatedBy?: string,
    ) {
        this.id = id;
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