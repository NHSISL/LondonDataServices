"use strict";
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.terminologyArtifactViewService = void 0;
var react_1 = require("react");
var terminologyArtifactService_1 = require("../../foundations/terminologyArtifactService");
var terminologyArtifactsView_1 = require("../../../models/views/components/terminologyArtifacts/terminologyArtifactsView");
exports.terminologyArtifactViewService = {
    useCreateTerminologyArtifact: function () {
        return terminologyArtifactService_1.terminologyArtifactService.useCreateTerminologyArtifact();
    },
    useGetAllTerminologyArtifacts: function (searchTerm) {
        try {
            var query = '?$orderby=name';
            if (searchTerm) {
                query = query + "&$filter=contains(name,'".concat(searchTerm, "')");
            }
            var response_1 = terminologyArtifactService_1.terminologyArtifactService.useGetAllTerminologyArtifacts(query);
            var _a = (0, react_1.useState)([]), mappedTerminologyArtifacts = _a[0], setMappedTerminologyArtifacts_1 = _a[1];
            (0, react_1.useEffect)(function () {
                if (response_1.data) {
                    var terminologyArtifacts_1 = response_1.data.map(function (terminologyArtifact) {
                        return new terminologyArtifactsView_1.TerminologyArtifactView(terminologyArtifacts_1.id, terminologyArtifacts_1.fullUrl, terminologyArtifacts_1.resourceType, terminologyArtifacts_1.version, terminologyArtifacts_1.name, terminologyArtifacts_1.title, terminologyArtifacts_1.status, terminologyArtifacts_1.lastUpdated, terminologyArtifacts_1.isCore, terminologyArtifacts_1.isDownloaded, terminologyArtifacts_1.createdBy, terminologyArtifacts_1.createdDate, terminologyArtifacts_1.updatedBy, terminologyArtifacts_1.updatedDate);
                    });
                    setMappedTerminologyArtifacts_1(terminologyArtifacts_1);
                }
            }, [response_1.data]);
            return __assign({ mappedTerminologyArtifacts: mappedTerminologyArtifacts }, response_1);
        }
        catch (err) {
            throw err;
        }
    },
    useGetTerminologyArtifactById: function (id) {
        try {
            var query = "?$filter=id eq ".concat(id);
            var response_2 = terminologyArtifactService_1.terminologyArtifactService.useGetAllTerminologyArtifacts(query);
            var _a = (0, react_1.useState)(), mappedTerminologyArtifact = _a[0], setMappedTerminologyArtifact_1 = _a[1];
            (0, react_1.useEffect)(function () {
                if (response_2.data && response_2.data[0]) {
                    var terminologyArtifact = new terminologyArtifactsView_1.TerminologyArtifactView(response_2.data[0].id, response_2.data[0].fullUrl, response_2.data[0].resourceType, response_2.data[0].version, response_2.data[0].name, response_2.data[0].title, response_2.data[0].status, response_2.data[0].lastUpdated, response_2.data[0].isCore, response_2.data[0].isDownloaded, response_2.data[0].createdBy, response_2.data[0].createdDate, response_2.data[0].updatedBy, response_2.data[0].updatedDate);
                    setMappedTerminologyArtifact_1(terminologyArtifact);
                }
            }, [response_2.data]);
            return __assign({ mappedTerminologyArtifact: mappedTerminologyArtifact }, response_2);
        }
        catch (err) {
            throw err;
        }
    },
    useUpdateTerminologyArtifact: function () {
        return terminologyArtifactService_1.terminologyArtifactService.useUpdateTerminologyArtifact();
    },
    useRemoveTerminologyArtifact: function () {
        return terminologyArtifactService_1.terminologyArtifactService.useDeleteTerminologyArtifact();
    },
};
//# sourceMappingURL=terminologyArtifactViewService.js.map