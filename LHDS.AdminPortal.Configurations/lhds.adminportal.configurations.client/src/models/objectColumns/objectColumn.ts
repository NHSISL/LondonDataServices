import { SpecificationObject } from '../specificationObjects/specificationObject';

export class ObjectColumn {
    public id: string;
    public specificationObjectId: string;
    public supplierColumnName: string;
    public ourColumnName: string;
    public columnDescription: string;
    public ordinalPosition: number;
    public populatedBy: string;
    public fhirDataType: string;
    public sqlDataType: string;
    public length: number;
    public precision: number;
    public scale: number;
    public supplierDateFormat: string;
    public isWatermark: boolean;
    public isSequencing: boolean;
    public isBusinessKey: boolean;
    public isUniqueRecordKey: boolean;
    public isVersionHashElement: boolean;
    public isSenderCode: boolean;
    public isAuthorCode: boolean;
    public isRelatedOrganisationId: boolean;
    public isDeleteFlag: boolean;
    public isPersonConfidentialData: boolean;
    public personConfidentialDataType: string;
    public maskingMethod: string;
    public isSensitiveRecordMarker: boolean;
    public codeSystem: string;
    public partitionColumnLevel: string;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;
    public specificationObject?: SpecificationObject;

    constructor(objectColumn: ObjectColumn) {
        this.id = objectColumn.id ? objectColumn.id : "";
        this.specificationObjectId = objectColumn.specificationObjectId;
        this.supplierColumnName = objectColumn.supplierColumnName;
        this.ourColumnName = objectColumn.ourColumnName;
        this.columnDescription = objectColumn.columnDescription;
        this.ordinalPosition = objectColumn.ordinalPosition;
        this.populatedBy = objectColumn.populatedBy;
        this.sqlDataType = objectColumn.sqlDataType;
        this.length = objectColumn.length;
        this.precision = objectColumn.precision;
        this.scale = objectColumn.scale;
        this.fhirDataType = objectColumn.fhirDataType;
        this.supplierDateFormat = objectColumn.supplierDateFormat;
        this.isWatermark = objectColumn.isWatermark;
        this.isSequencing = objectColumn.isSequencing;
        this.isBusinessKey = objectColumn.isBusinessKey;
        this.isUniqueRecordKey = objectColumn.isUniqueRecordKey;
        this.isVersionHashElement = objectColumn.isVersionHashElement;
        this.isSenderCode = objectColumn.isSenderCode;
        this.isAuthorCode = objectColumn.isAuthorCode;
        this.isRelatedOrganisationId = objectColumn.isRelatedOrganisationId;
        this.isDeleteFlag = objectColumn.isDeleteFlag;
        this.isSensitiveRecordMarker = objectColumn.isSensitiveRecordMarker;
        this.isPersonConfidentialData = objectColumn.isPersonConfidentialData;
        this.personConfidentialDataType = objectColumn.personConfidentialDataType;
        this.maskingMethod = objectColumn.maskingMethod;
        this.codeSystem = objectColumn.codeSystem;
        this.partitionColumnLevel = objectColumn.partitionColumnLevel;
        this.createdBy = objectColumn.createdBy;
        this.createdDate = new Date(objectColumn.createdDate);
        this.updatedBy = objectColumn.updatedBy;
        this.updatedDate = new Date(objectColumn.updatedDate);

        if (objectColumn.specificationObject !== undefined && objectColumn.specificationObject !== null) {
            this.specificationObject = new SpecificationObject(objectColumn.specificationObject);
        }
    }
}