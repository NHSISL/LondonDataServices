"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TerminologyArtifactHomeViewService = void 0;
var react_1 = require("react");
var terminologyArtifactHomeView_1 = require("../../../models/terminologyArtifacts/terminologyArtifactHomeView");
var terminologyArtifactService_1 = require("../../foundations/terminologyArtifactService");
exports.TerminologyArtifactHomeViewService = {
    useGetAllTerminologyArtifacts: function (searchTerm) {
        try {
            var query = "?$orderby=createdDate";
            if (searchTerm) {
                query = query + "&$filter=contains(name,'".concat(searchTerm, "')");
            }
            var response_1 = terminologyArtifactService_1.terminologyArtifactService.useGetAllTerminologyArtifactsPages(query);
            var _a = (0, react_1.useState)(), mappedTerminologyArtifacts = _a[0], setMappedTerminologyArtifacts_1 = _a[1];
            var _b = (0, react_1.useState)([]), pages = _b[0], setPages_1 = _b[1];
            (0, react_1.useEffect)(function () {
                if (response_1.data && response_1.data.pages) {
                    var terminologyArtifactArray_1 = [];
                    response_1.data.pages.forEach(function (page) {
                        page.data.forEach(function (terminologyArtifact) {
                            terminologyArtifactArray_1.push(new terminologyArtifactHomeView_1.TerminologyArtifactHomeView(terminologyArtifact.id, terminologyArtifact.fullUrl, terminologyArtifact.resourceType, terminologyArtifact.version, terminologyArtifact.name, terminologyArtifact.title, terminologyArtifact.status, terminologyArtifact.lastUpdated, terminologyArtifact.isCore, terminologyArtifact.isDownloaded, terminologyArtifact.createdBy, terminologyArtifact.createdDate, terminologyArtifact.updatedBy, terminologyArtifact.updatedDate));
                        });
                    });
                    setMappedTerminologyArtifacts_1(terminologyArtifactArray_1);
                    setPages_1(response_1.data.pages);
                }
            }, [response_1.data]);
            return {
                mappedTerminologyArtifacts: mappedTerminologyArtifacts,
                pages: pages,
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
};
//# sourceMappingURL=terminologyArtifactHomeViewService.js.map