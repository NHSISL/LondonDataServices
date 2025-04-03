import { Guid } from 'guid-typescript';
import { SpecificationObject } from '../../../specificationObjects/specificationObject';

export class ObjectColumnView {
    public id: Guid;
    public specificationObjectId: Guid;
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
    public specificationObject?: SpecificationObject

    constructor(
        id: Guid,
        specificationObjectId: Guid,
        supplierColumnName?: string,
        ourColumnName?: string,
        columnDescription?: string,
        ordinalPosition?: number,
        populatedBy?: string,
        fhirDataType?: string,
        sqlDataType?: string,
        length?: number,
        precision?: number,
        scale?: number,
        supplierDateFormat?: string,
        isWatermark?: boolean,
        isSequencing?: boolean,
        isBusinessKey?: boolean,
        isUniqueRecordKey?: boolean,
        isVersionHashElement?: boolean,
        isSenderCode?: boolean,
        isAuthorCode?: boolean,
        isRelatedOrganisationId?: boolean,
        isDeleteFlag?: boolean,
        isPersonConfidentialData?: boolean,
        personConfidentialDataType?: string,
        maskingMethod?: string,
        isSensitiveRecordMarker?: boolean,
        codeSystem?: string,
        partitionColumnLevel?: string,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
        specificationObject?: SpecificationObject
    ) {
        this.id = id
        this.specificationObjectId = specificationObjectId;
        this.supplierColumnName = supplierColumnName || "";
        this.ourColumnName = ourColumnName || "";
        this.columnDescription = columnDescription || "";
        this.ordinalPosition = ordinalPosition || 0;
        this.populatedBy = populatedBy || "";
        this.fhirDataType = fhirDataType || "";
        this.sqlDataType = sqlDataType || "";
        this.length = length || 0;
        this.precision = precision || 0;
        this.scale = scale || 0;
        this.supplierDateFormat = supplierDateFormat || "";
        this.isWatermark = isWatermark || false;
        this.isSequencing = isSequencing || false;
        this.isBusinessKey = isBusinessKey || false;
        this.isUniqueRecordKey = isUniqueRecordKey || false;
        this.isVersionHashElement = isVersionHashElement || false;
        this.isSenderCode = isSenderCode || false;
        this.isAuthorCode = isAuthorCode || false;
        this.isRelatedOrganisationId = isRelatedOrganisationId || false;
        this.isDeleteFlag = isDeleteFlag || false;
        this.isPersonConfidentialData = isPersonConfidentialData || false;
        this.personConfidentialDataType = personConfidentialDataType || "";
        this.maskingMethod = maskingMethod || "";
        this.isSensitiveRecordMarker = isSensitiveRecordMarker || false;
        this.codeSystem = codeSystem || "";
        this.partitionColumnLevel = partitionColumnLevel || "";
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
        this.specificationObject = specificationObject;
    }
}