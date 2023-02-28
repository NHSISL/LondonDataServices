import { Guid } from 'guid-typescript';
import { Audit } from '../audits/audit';

export class IngestionTracking {
    public id: Guid;
    public source: string;
    public encryptedFileName: string;
    public decryptedFileName: string;
    public decrypted: boolean;
    public lastSeen: Date;
    public fileDeleted: boolean;
    public recordCount: number;
    public encryptedFileSize: number;
    public decryptedFileSize: number;
    public audit: Audit;

    constructor(ingestionTracking: any) {
        this.id = ingestionTracking.id ? Guid.parse(ingestionTracking.id) : Guid.parse(Guid.EMPTY);
        this.source = ingestionTracking.source || "";
        this.encryptedFileName = ingestionTracking.encryptedFileName || "";
        this.decryptedFileName = ingestionTracking.decryptedFileName || "";
        this.decrypted = ingestionTracking.decrypted;
        this.lastSeen = ingestionTracking.lastSeen ? new Date(ingestionTracking.lastSeen) : new Date();
        this.fileDeleted = ingestionTracking.fileDeleted;
        this.recordCount = ingestionTracking.recordCount;
        this.encryptedFileSize = ingestionTracking.encryptedFileSize;
        this.decryptedFileSize = ingestionTracking.decryptedFileSize;
        this.audit = ingestionTracking.audit;
    }
}