import { Guid } from 'guid-typescript';

export class TerminologyArtifactView {
    public id: Guid;
    public fullUrl?: string;
    public resourceType?: string;
    public version?: string;
    public name?: string;
    public title?: string;
    public status?: string;
    public lastUpdated?: Date;
    public isCore?: boolean;
    public isDownloaded?: boolean;
    public isForUser?: boolean;
    public isDownloadedForUser?: boolean;
    public isError?: boolean;
    public errorMessage?: string;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        fullUrl?: string,
        resourceType?: string,
        version?: string,
        name?: string,
        title?: string,
        status?: string,
        lastUpdated?: Date,
        isCore?: boolean,
        isDownloaded?: boolean,
        isForUser?: boolean,
        isDownloadedForUser?: boolean,
        isError?: boolean,
        errorMessage?: string,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
    ) {
        this.id = id;
        this.fullUrl = fullUrl || "";
        this.resourceType = resourceType || "";
        this.version = version || "";
        this.name = name || "";;
        this.title = title || "";
        this.status = status || "";
        this.lastUpdated = lastUpdated;
        this.isCore = isCore;
        this.isDownloaded = isDownloaded;
        this.isForUser = isForUser;
        this.isDownloadedForUser = isDownloadedForUser;
        this.isError = isError;
        this.errorMessage = errorMessage;
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate;
        this.updatedBy = updatedBy !== undefined ? updatedBy : ''
        this.updatedDate = updatedDate;   
    }
}