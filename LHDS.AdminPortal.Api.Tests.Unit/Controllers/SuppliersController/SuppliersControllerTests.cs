// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using LHDS.Core.Services.Foundations.Suppliers;
using Moq;
using RESTFulSense.Controllers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.AdminPortal.Api.Controllers;

namespace LHDS.AdminPortal.Api.Tests.Unit.Controllers.Suppliers
{
    public partial class SuppliersControllerTests : RESTFulController
    {
        private readonly Mock<ISupplierService> supplierServiceMock;
        private readonly SuppliersController suppliersController;

        public SuppliersControllerTests()
        {
            this.supplierServiceMock = new Mock<ISupplierService>();
            this.suppliersController = new SuppliersController(this.supplierServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<Supplier> CreateRandomSuppliers() =>
            CreateSupplierFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static Supplier CreateRandomSupplier() =>
            CreateSupplierFiller().Create();

        private static Filler<Supplier> CreateSupplierFiller()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(accessAudit => accessAudit.CreatedBy).Use(user)
                .OnProperty(accessAudit => accessAudit.UpdatedBy).Use(user);

            return filler;
        }

        public static TheoryData<Xeption> ServerExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new SupplierDependencyException(
                    message: someMessage,
                    innerException: someInnerException),

                new SupplierServiceException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }

        public static TheoryData<Xeption> ValidationExceptions()
        {
            var someInnerException = new Xeption();
            string someMessage = GetRandomString();

            return new TheoryData<Xeption>
            {
                new SupplierValidationException(
                    message: someMessage,
                    innerException: someInnerException),

                new SupplierDependencyValidationException(
                    message: someMessage,
                    innerException: someInnerException)
            };
        }
    }
}