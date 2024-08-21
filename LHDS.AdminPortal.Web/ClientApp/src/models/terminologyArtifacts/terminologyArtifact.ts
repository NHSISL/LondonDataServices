import { Guid } from 'guid-typescript';

export class TerminologyArtifact {
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

    constructor(terminologyArtifact: any) {
        this.id = terminologyArtifact.id ? Guid.parse(terminologyArtifact.id) : Guid.parse(Guid.EMPTY);
        this.fullUrl = terminologyArtifact.fullUrl;
        this.resourceType = terminologyArtifact.resourceType;
        this.version = terminologyArtifact.version;
        this.name = terminologyArtifact.name;
        this.title = terminologyArtifact.title;
        this.status = terminologyArtifact.status;
        this.lastUpdated = new Date(terminologyArtifact.lastUpdated);
        this.isCore = terminologyArtifact.isCore;
        this.isDownloaded = terminologyArtifact.isDownloaded;
        this.isForUser = terminologyArtifact.isForUser;
        this.isDownloadedForUser = terminologyArtifact.isDownloadedForUser;
        this.isError = terminologyArtifact.isError;
        this.errorMessage = terminologyArtifact.errorMessage;
        this.createdBy = terminologyArtifact.createdBy;
        this.createdDate = new Date(terminologyArtifact.createdDate);
        this.updatedBy = terminologyArtifact.updatedBy;
        this.updatedDate = new Date(terminologyArtifact.updatedDate);
    }
}