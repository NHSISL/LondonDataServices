import { Guid } from 'guid-typescript';

export class ResolvedAddressView {
    public id: Guid;
    public uprn?: string;
    public usrn?: string;
    public postCode?: string;
    public retryCount?: number;
    public isProcessing: boolean;
    public uniqueReference?: string;
    public unstructuredPostalAddress?: string;
    public alternateUnstructuredPostalAddress?: string | null;
    public batchReference?: string;
    public addressFormatQuality?: string;
    public algorithm?: string;
    public buildingName?: string;
    public buildingNumber?: string;
    public classification?: string;
    public departmentName?: string;
    public dependentLocality?: string;
    public dependentThoroughfare?: string;
    public doubleDependentLocality?: string;
    public matchPattern?: string;
    public matchedWithAssign?: boolean;
    public organisationName?: string;
    public postCodeQuality?: string;
    public postTown?: string;
    public qualifier?: string;
    public subBuildingName?: string;
    public thoroughfare?: string;
    public isExported: boolean;
    public isProcessed: boolean;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        uprn?: string,
        usrn?: string,
        postCode?: string,
        retryCount?: number,
        isProcessing?: boolean,
        uniqueReference?: string,
        unstructuredPostalAddress?: string,
        alternateUnstructuredPostalAddress?: string,
        batchReference?: string,
        addressFormatQuality?: string,
        algorithm?: string,
        buildingName?: string,
        buildingNumber?: string,
        classification?: string,
        departmentName?: string,
        dependentLocality?: string,
        dependentThoroughfare?: string,
        doubleDependentLocality?: string,
        matchPattern?: string,
        matchedWithAssign?: boolean,
        organisationName?: string,
        postCodeQuality?: string,
        postTown?: string,
        qualifier?: string,
        subBuildingName?: string,
        thoroughfare?: string,
        isExported?: boolean,
        isProcessed?: boolean,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date) {
        this.id = id;
        this.uprn = uprn || "";
        this.usrn = usrn || "";
        this.postCode = postCode || "";
        this.retryCount = retryCount;
        this.isProcessing = isProcessing === true ? true : false;;
        this.uniqueReference = uniqueReference || "";
        this.unstructuredPostalAddress = unstructuredPostalAddress || "";
        this.alternateUnstructuredPostalAddress = alternateUnstructuredPostalAddress ?? null;
        this.batchReference = batchReference || "";
        this.addressFormatQuality = addressFormatQuality || "";
        this.algorithm = algorithm || "";
        this.buildingName = buildingName || "";
        this.buildingNumber = buildingNumber || "";
        this.classification = classification || "";
        this.departmentName = departmentName || "";
        this.dependentLocality = dependentLocality || "";
        this.dependentThoroughfare = dependentThoroughfare || "";
        this.doubleDependentLocality = doubleDependentLocality || "";
        this.matchPattern = matchPattern || "";
        this.matchedWithAssign = matchedWithAssign === true ? true : false;
        this.organisationName = organisationName || "";
        this.postCodeQuality = postCodeQuality || "";
        this.postTown = postTown || "";
        this.qualifier = qualifier || "";
        this.subBuildingName = subBuildingName || "";
        this.thoroughfare = thoroughfare || "";
        this.isExported = isExported === true ? true : false;;
        this.isProcessed = isProcessed === true ? true : false;;
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate;
        this.updatedBy = updatedBy !== undefined ? updatedBy : '';
        this.updatedDate = updatedDate;
    }
}