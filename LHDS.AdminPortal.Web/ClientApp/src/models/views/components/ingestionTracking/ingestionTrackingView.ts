import { Guid } from "guid-typescript";

export class IngestionTrackingView {
    public id: Guid;
    public fileName: string;
    public source?: string;
    public encryptedFileName?: string;
    public decryptedFileName?: string;
    public decrypted?: boolean;
    public lastSeen?: Date;
    public fileDeleted?: boolean;
    public recordCount?: number;
    public encryptedFileSize?: number;
    public decryptedFileSize?: number;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        fileName: string,
        source?: string,
        encryptedFileName?: string,
        decryptedFileName?: string,
        decrypted?: boolean,
        lastSeen?: Date,
        fileDeleted?: boolean,
        recordCount?: number,
        encryptedFileSize?: number,
        decryptedFileSize?: number,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
    ) {
        this.id = id;
        this.fileName = fileName || "";
        this.source = source || "";
        this.encryptedFileName = encryptedFileName || "";
        this.decryptedFileName = decryptedFileName || "";
        this.decrypted = decrypted;
        this.lastSeen = lastSeen;
        this.fileDeleted = fileDeleted;
        this.recordCount = recordCount;
        this.encryptedFileSize = encryptedFileSize;
        this.decryptedFileSize = decryptedFileSize;
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate;
        this.updatedBy = updatedBy !== undefined ? updatedBy : ''
        this.updatedDate = updatedDate;
    }
}