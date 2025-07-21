import { Guid } from 'guid-typescript';

export class AddressView {
    public id: Guid;
    public isProcessing: boolean;
    public isSynced: boolean;
    public uprn?: string;
    public usrn?: string;
    public organisationName?: string;
    public departmentName?: string;
    public subBuildingName?: string;
    public buildingName?: string;
    public BuildingNumber?: string;
    public dependentThoroughfare?: string;
    public thoroughfare?: string;
    public doubleDependentLocality?: string;
    public dependentLocality?: string;
    public postTown?: string;
    public postCode?: string;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        isProcessing: boolean,
        isSynced: boolean,
        uprn?: string,
        usrn?: string,
        organisationName?: string,
        departmentName?: string,
        subBuildingName?: string,
        buildingName?: string,
        BuildingNumber?: string,
        dependentThoroughfare?: string,
        thoroughfare?: string,
        doubleDependentLocality?: string,
        dependentLocality?: string,
        postTown?: string,
        postCode?: string,  
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date
    ) {
        this.id = id;
        this.uprn = uprn || "";
        this.usrn = usrn || "";
        this.organisationName = organisationName || "";
        this.departmentName = departmentName || "";
        this.subBuildingName = subBuildingName || "";
        this.buildingName = buildingName || "";
        this.BuildingNumber = BuildingNumber || "";
        this.dependentThoroughfare = dependentThoroughfare || "";
        this.thoroughfare = thoroughfare || "";
        this.doubleDependentLocality = doubleDependentLocality || "";
        this.dependentLocality = dependentLocality || "";
        this.postTown = postTown || "";
        this.postCode = postCode || "";
        this.isProcessing = isProcessing === true ? true : false;
        this.isSynced = isSynced === true ? true : false;
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate;
        this.updatedBy = updatedBy !== undefined ? updatedBy : '';
        this.updatedDate = updatedDate;
    }
}