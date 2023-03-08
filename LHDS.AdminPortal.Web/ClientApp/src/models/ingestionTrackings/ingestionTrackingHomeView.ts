import { Guid } from 'guid-typescript';
import { Audit } from '../audits/audit';

export class IngestionTrackingHomeView {
    public id: string;
    public fileName: string;
    public supplierId: string;
    public encryptedFileName: string;
    public decryptedFileName: string;
    public decrypted: boolean;
    public lastSeen: Date;
    public fileDeleted: boolean;
    public recordCount: number;
    public encryptedFileSize: number;
    public decryptedFileSize: number;
    public audit: Audit;

    constructor(
        id: string,
        fileName: string,
        supplierId: string,
        encryptedFileName: string,
        decryptedFileName: string,
        decrypted: boolean,
        lastSeen: Date,
        fileDeleted: boolean,
        recordCount: number,
        encryptedFileSize: number,
        decryptedFileSize: number,
        audit: Audit
    ) {
        this.id = id;
        this.fileName = fileName;
        this.supplierId = supplierId;
        this.encryptedFileName = encryptedFileName;
        this.decryptedFileName = decryptedFileName;
        this.decrypted = decrypted === true ? true : false;
        this.lastSeen = lastSeen;
        this.fileDeleted = fileDeleted;
        this.recordCount = recordCount;
        this.encryptedFileSize = encryptedFileSize;
        this.decryptedFileSize = decryptedFileSize;
        this.audit = audit;
    }
}
