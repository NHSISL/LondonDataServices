import { Guid } from 'guid-typescript';
import { Audit } from '../audits/audit';

export class IngestionTracking {
    public id: Guid;
    public fileName: string;
    public source: string;
    public encryptedFileName: string;
    public decryptedFileName: string;
    public decrypted: boolean;
    public lastSeen: Date;
    public fileDeleted: boolean;
    public recordCount: number;
    public encryptedFileSize: number;
    public decryptedFileSize: number;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;
    public audit: Audit;

    constructor(ingestionTracking: any) {
        this.id = ingestionTracking.id ? Guid.parse(ingestionTracking.id) : Guid.parse(Guid.EMPTY);
        this.fileName = ingestionTracking.fileName || "";
        this.source = ingestionTracking.source || "";
        this.encryptedFileName = ingestionTracking.encryptedFileName || "";
        this.decryptedFileName = ingestionTracking.decryptedFileName || "";
        this.decrypted = ingestionTracking.decrypted;
        this.lastSeen = ingestionTracking.lastSeen ? new Date(ingestionTracking.lastSeen) : new Date();
        this.fileDeleted = ingestionTracking.fileDeleted;
        this.recordCount = ingestionTracking.recordCount;
        this.encryptedFileSize = ingestionTracking.encryptedFileSize;
        this.decryptedFileSize = ingestionTracking.decryptedFileSize;
        this.createdBy = ingestionTracking.createdBy !== undefined ? ingestionTracking.createdBy : '';
        this.createdDate = ingestionTracking.createdDate;
        this.updatedBy = ingestionTracking.updatedBy !== undefined ? ingestionTracking.updatedBy : ''
        this.updatedDate = ingestionTracking.updatedDate;
        this.audit = ingestionTracking.audit;
    }
}