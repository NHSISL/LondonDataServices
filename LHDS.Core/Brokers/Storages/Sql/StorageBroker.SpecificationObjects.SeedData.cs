// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        private static void AddSpecificationObjectSeedData(ModelBuilder modelBuilder)
        {
            List<SpecificationObject> specificationObjects = new List<SpecificationObject>
            {
                new SpecificationObject
                {
                    Id = Guid.Parse("E44E12A4-DF37-401E-AFC9-08024BE3991A"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Admin_Location",
                    OurObjectName = "Admin_Location",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("07009820-23AF-4FF1-A768-0CD90E22B0D4"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Agreements_SharingOrganisation",
                    OurObjectName = "Agreements_SharingOrganisation",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("97C6ABCB-94EA-4EDE-9F7D-1856D4C776BA"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Audit_PatientAudit",
                    OurObjectName = "Audit_PatientAudit",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("DFB6DB14-012E-40EB-B8B2-26163DB58E06"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "CareRecord_Consultation",
                    OurObjectName = "CareRecord_Consultation",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("94265244-ABF3-4E54-A9E2-38E46F6AC485"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Prescribing_DrugRecord",
                    OurObjectName = "Prescribing_DrugRecord",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("B9BA8645-9307-4CF4-8336-519F0E685BF7"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Prescribing_IssueRecord",
                    OurObjectName = "Prescribing_IssueRecord",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("93BA6EE2-33B9-4802-BAA3-5379A45A3FA3"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Appointment_Session",
                    OurObjectName = "Appointment_Session",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("B3BFFEC5-2A51-438C-9259-5B5DAF338384"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Coding_DrugCode",
                    OurObjectName = "Coding_DrugCode",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("BE4E6870-A802-4504-8B41-5FF3AD8EF74B"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "CareRecord_Diary",
                    OurObjectName = "CareRecord_Diary",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("5CEC2ECC-4538-487C-8168-6052E06AE233"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Coding_ClinicalCode",
                    OurObjectName = "Coding_ClinicalCode",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("4B4DB138-A64E-47D0-83A0-68EC97B5AC8A"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "CareRecord_Observation",
                    OurObjectName = "CareRecord_Observation",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("41073B65-0CFD-4CED-BD4D-75DEF2BB0977"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Admin_OrganisationLocation",
                    OurObjectName = "Admin_OrganisationLocation",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("445D0610-0985-4F04-83AA-990AE7EC6EA8"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Admin_Organisation",
                    OurObjectName = "Admin_Organisation",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("4A392C33-E268-4D4F-A474-9A5B6C803F12"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "CareRecord_ObservationReferral",
                    OurObjectName = "CareRecord_ObservationReferral",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("28A1F2DF-0DEF-46DA-B319-A08D29B9C7B6"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Admin_Patient",
                    OurObjectName = "Admin_Patient",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("01170C3B-F7EF-4C02-B7F0-A251A25F470B"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Appointment_Slot",
                    OurObjectName = "Appointment_Slot",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("C89D4511-2697-4A1C-BB28-B50CF4F88A34"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Audit_RegistrationAudit",
                    OurObjectName = "Audit_RegistrationAudit",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("699E9F7A-9CA1-482C-8C97-BB996F146FF3"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Admin_UserInRole",
                    OurObjectName = "Admin_UserInRole",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("BD125C16-E020-49BA-8FD6-D1CE8201D5DB"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "CareRecord_Problem",
                    OurObjectName = "CareRecord_Problem",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                },
                new SpecificationObject
                {
                    Id = Guid.Parse("D506DC20-115A-4BD0-88F0-DA6DECBEA70A"),
                    DataSetSpecificationId = Guid.Parse("E8EBCE80-E619-40CA-B45F-9C3AC0328143"),
                    SupplierObjectName = "Appointment_SessionUser",
                    OurObjectName = "Appointment_SessionUser",
                    IsPushedToUs = false,
                    IsPulledByUs = false,
                    IsSubmissionHeaderObject = false,
                    IsTransactionLog = false,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                }
            };

            modelBuilder.Entity<SpecificationObject>().HasData(specificationObjects);
        }
    }
}
