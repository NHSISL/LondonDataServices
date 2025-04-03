import { Supplier } from '../suppliers/supplier';

export class IngestionTracking {
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
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;
    public supplier?: Supplier;

    constructor(ingestionTracking: IngestionTracking) {
        this.id = ingestionTracking.id;
        this.fileName = ingestionTracking.fileName || "";
        this.supplierId = ingestionTracking.supplierId || "";
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

        if (ingestionTracking.supplier !== undefined && ingestionTracking.supplier !== null) {
            this.supplier = new Supplier(ingestionTracking.supplier);
        }
    }
}
