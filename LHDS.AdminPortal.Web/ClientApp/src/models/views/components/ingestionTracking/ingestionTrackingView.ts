import { Supplier } from "../../../suppliers/supplier";

export class IngestionTrackingView {
    public id: string;
    public fileName: string;
    public supplierId: string;
    public encryptedFileName: string;
    public decryptedFileName: string;
    public decrypted: boolean;
    public lastSeen: Date;
    public fileDeleted: boolean;
    public encryptedFileSize: number;
    public decryptedFileSize: number;
    public isDownloaded: boolean;
    public isProcessing: boolean;
    public retryCount: number;
    public sourceFolderPath: string;
    public lastAttemptedDate: Date;
    public dataSetSpecificationId: string;
    public batch: string;
    public objectName: string;
    public batchReadyFolderPath: string;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;
    public supplier?: Supplier;

    constructor(
        id: string,
        fileName: string,
        supplierId: string,
        encryptedFileName: string,
        decryptedFileName: string,
        decrypted: boolean,
        lastSeen: Date,
        fileDeleted: boolean,
        encryptedFileSize: number,
        decryptedFileSize: number,
        isDownloaded: boolean,
        isProcessing: boolean,
        retryCount: number,
        sourceFolderPath: string,
        lastAttemptedDate: Date,
        dataSetSpecificationId: string,
        batch: string,
        objectName: string,
        batchReadyFolderPath: string,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
        supplier?: Supplier,
    ) {
        this.id = id;
        this.fileName = fileName || "";
        this.supplierId = supplierId || "";
        this.encryptedFileName = encryptedFileName || "";
        this.decryptedFileName = decryptedFileName || "";
        this.decrypted = decrypted;
        this.lastSeen = lastSeen;
        this.fileDeleted = fileDeleted;
        this.encryptedFileSize = encryptedFileSize;
        this.decryptedFileSize = decryptedFileSize;
        this.isDownloaded = isDownloaded;
        this.isProcessing = isProcessing;
        this.retryCount = retryCount;
        this.sourceFolderPath = sourceFolderPath || "";
        this.lastAttemptedDate = lastAttemptedDate;
        this.dataSetSpecificationId = dataSetSpecificationId || "";
        this.batch = batch || "";
        this.objectName = objectName || "";
        this.batchReadyFolderPath = batchReadyFolderPath || "";
        this.createdBy = createdBy;
        this.createdDate = createdDate;
        this.updatedBy = updatedBy;
        this.updatedDate = updatedDate;

        if (supplier !== undefined && supplier !== null) {
            this.supplier = new Supplier(supplier);
        }
    }
}