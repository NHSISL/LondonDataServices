import { Guid } from 'guid-typescript';
import { Audit } from '../audits/audit';

export class IngestionTrackingHomeView {
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
    public audit: Audit;

    constructor(
        id: Guid,
        fileName: string,
        source: string,
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
        this.fileName = fileName 
        this.source = source 
        this.encryptedFileName = encryptedFileName 
        this.decryptedFileName = decryptedFileName 
        this.decrypted = decrypted === true ? true : false;
        this.lastSeen = lastSeen
        this.fileDeleted = fileDeleted;
        this.recordCount = recordCount;
        this.encryptedFileSize = encryptedFileSize;
        this.decryptedFileSize = decryptedFileSize;
        this.audit = audit;
    }
}