import { Guid } from 'guid-typescript';

export class IngestionTrackingView {
    public id: string;
    public source: string;
    public encryptedFileName: string;
    public decryptedFileName: string;
    public decrypted: boolean;
    public lastSeen: Date;
    public fileDeleted: boolean;
    public recordCount: number;
    public encryptedFileSize: number;
    public decryptedFileSize: number;

    constructor(
        id: string,
        source: string,
        encryptedFileName: string,
        decryptedFileName: string,
        decrypted: boolean,
        lastSeen: Date,
        fileDeleted: boolean,
        recordCount: number,
        encryptedFileSize: number,
        decryptedFileSize: number
    ) {
        this.id = id;
        this.source = source || "";
        this.encryptedFileName = encryptedFileName || "";
        this.decryptedFileName = decryptedFileName || "";
        this.decrypted = decrypted;
        this.lastSeen = lastSeen;
        this.fileDeleted = fileDeleted;
        this.recordCount = recordCount;
        this.encryptedFileSize = encryptedFileSize;
        this.decryptedFileSize = decryptedFileSize;
    }
}