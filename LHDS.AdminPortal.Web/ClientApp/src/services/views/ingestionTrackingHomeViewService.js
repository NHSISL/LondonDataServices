"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ingestionTrackingHomeViewService = void 0;
var react_1 = require("react");
var ingestionTrackingHomeView_1 = require("../../models/ingestionTrackings/ingestionTrackingHomeView");
var ingestionTrackingService_1 = require("../foundations/ingestionTrackingService");
var landingService_1 = require("../foundations/landingService");
var decryptionService_1 = require("../foundations/decryptionService");
var documentService_1 = require("../foundations/documentService");
exports.ingestionTrackingHomeViewService = {
    useGetAllIngestionTrackings: function (searchTerm, supplierId) {
        try {
            var query = "?$orderby=createdDate desc&$expand=supplier";
            if (searchTerm) {
                query = query + "&$filter=contains(fileName,'".concat(searchTerm, "') or contains(decryptedFileName,'").concat(searchTerm, "')");
            }
            if (supplierId) {
                query = query + "&$filter=supplier/id eq ".concat(supplierId);
            }
            var response_1 = ingestionTrackingService_1.ingestionTrackingService.useGetAllIngestionTrackingPages(query);
            var _a = (0, react_1.useState)(), mappedIngestionTrackings = _a[0], setMappedIngestionTrackings_1 = _a[1];
            var _b = (0, react_1.useState)([]), pages = _b[0], setPages_1 = _b[1];
            var _c = (0, react_1.useState)([]), supplierOptions = _c[0], setSupplierOptions = _c[1];
            var _d = (0, react_1.useState)(), selectedOption = _d[0], setSelectedOption_1 = _d[1];
            (0, react_1.useEffect)(function () {
                if (response_1.data && response_1.data.pages) {
                    var ingestionTrackings_1 = [];
                    response_1.data.pages.forEach(function (x) {
                        x.data.forEach(function (ingestionTracking) {
                            ingestionTrackings_1.push(new ingestionTrackingHomeView_1.IngestionTrackingHomeView(ingestionTracking.id, ingestionTracking.fileName, ingestionTracking.supplierId, ingestionTracking.encryptedFileName, ingestionTracking.decryptedFileName, ingestionTracking.decrypted, ingestionTracking.lastSeen, ingestionTracking.fileDeleted, ingestionTracking.recordCount, ingestionTracking.encryptedFileSize, ingestionTracking.decryptedFileSize, ingestionTracking.createdBy, ingestionTracking.createdDate, ingestionTracking.updatedBy, ingestionTracking.updatedDate, ingestionTracking.supplier));
                        });
                    });
                    setMappedIngestionTrackings_1(ingestionTrackings_1);
                    setPages_1(response_1.data.pages);
                }
            }, [response_1.data, setSupplierOptions]);
            var handleSupplierChange = function (event) {
                setSelectedOption_1(event.target.value);
            };
            return {
                mappedIngestionTrackings: mappedIngestionTrackings,
                pages: pages,
                supplierOptions: supplierOptions,
                selectedOption: selectedOption,
                handleSupplierChange: handleSupplierChange,
                isLoading: response_1.isLoading,
                fetchNextPage: response_1.fetchNextPage,
                isFetchingNextPage: response_1.isFetchingNextPage,
                hasNextPage: !!response_1.hasNextPage,
                data: response_1.data,
                refetch: response_1.refetch
            };
        }
        catch (err) {
            throw err;
        }
    },
    useRelandIngestionTracking: function () {
        return landingService_1.landingService.useGetDownloadLinkByFileName();
    },
    useRedecryptIngestionTracking: function () {
        return decryptionService_1.decryptionService.useGetDocumentByFileNameToDecryptAsync();
    },
    useDownloadEncryptedDocument: function () {
        return documentService_1.documentService.useGetDownloadLinkByFileName();
    },
    useDownloadDecryptedDocument: function () {
        return documentService_1.documentService.useGetDownloadLinkByFileName();
    },
};
//# sourceMappingURL=ingestionTrackingHomeViewService.js.map