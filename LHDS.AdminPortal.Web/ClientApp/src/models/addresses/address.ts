import { Guid } from 'guid-typescript';

export class Address {
    public id: Guid;
    public uprn?: string;
    public usrn?: string;
    public organisationName?: string;
    public departmentName?: string;
    public subBuildingName?: string;
    public buildingName?: string;
    public buildingNumber?: string;
    public dependentThoroughfare?: string;
    public thoroughfare?: string;
    public doubleDependentLocality?: string;
    public dependentLocality?: string;
    public postTown?: string;
    public postCode?: string;
    public isProcessing: boolean;
    public isSynced: boolean;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(address: any) {
        this.id = address.id ? Guid.parse(address.id) : Guid.parse(Guid.EMPTY);
        this.uprn = address.uprn || "";
        this.usrn = address.usrn || "";
        this.organisationName = address.organisationName || "";
        this.departmentName = address.departmentName || "";
        this.subBuildingName = address.subBuildingName || "";
        this.buildingName = address.buildingName || "";
        this.buildingNumber = address.BuildingNumber || "";
        this.dependentThoroughfare = address.dependentThoroughfare || "";
        this.thoroughfare = address.thoroughfare || "";
        this.doubleDependentLocality = address.doubleDependentLocality || "";
        this.dependentLocality = address.dependentLocality || "";
        this.postTown = address.postTown || "";
        this.postCode = address.postCode || "";
        this.isProcessing = address.isProcessing === true ? true : false;
        this.isSynced = address.isSynced === true ? true : false;
        this.createdDate = address.createdDate;
        this.createdBy = address.createdBy;
        this.createdDate = new Date(address.createdDate);
        this.updatedBy = address.updatedBy;
        this.updatedDate = new Date(address.updatedDate);
    }
}