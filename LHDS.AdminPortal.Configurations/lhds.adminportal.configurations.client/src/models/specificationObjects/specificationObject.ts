import { DataSetSpecification } from '../dataSetSpecifications/dataSetSpecification';

export class SpecificationObject {
    public id: string;
    public dataSetSpecificationId: string;
    public supplierObjectName: string;
    public ourObjectName: string;
    public objectDescription: string;
    public interchangeProtocol: string;
    public isPushedToUs: boolean;
    public isPulledByUs: boolean;
    public deletionHandling: string;
    public isSubmissionHeaderObject: boolean;
    public isTransactionLog: boolean
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;
    public dataSetSpecification?: DataSetSpecification;

    constructor(dataSetObjects: SpecificationObject) {
        this.id = dataSetObjects.id ? dataSetObjects.id : "";
        this.dataSetSpecificationId = dataSetObjects.dataSetSpecificationId;
        this.supplierObjectName = dataSetObjects.supplierObjectName;
        this.ourObjectName = dataSetObjects.ourObjectName;
        this.objectDescription = dataSetObjects.objectDescription;
        this.interchangeProtocol = dataSetObjects.interchangeProtocol;
        this.isPushedToUs = dataSetObjects.isPushedToUs;
        this.isPulledByUs = dataSetObjects.isPulledByUs;
        this.deletionHandling = dataSetObjects.deletionHandling;
        this.isSubmissionHeaderObject = dataSetObjects.isSubmissionHeaderObject;
        this.isTransactionLog = dataSetObjects.isTransactionLog;
        this.createdDate = dataSetObjects.createdDate;
        this.createdBy = dataSetObjects.createdBy;
        this.createdDate = new Date(dataSetObjects.createdDate);
        this.updatedBy = dataSetObjects.updatedBy;
        this.updatedDate = new Date(dataSetObjects.updatedDate);

        if (dataSetObjects.dataSetSpecification !== undefined && dataSetObjects.dataSetSpecification !== null) {
            this.dataSetSpecification = new DataSetSpecification(dataSetObjects.dataSetSpecification);
        }
    }
}