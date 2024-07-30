"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.terminologyArtifactService = void 0;
var msal_react_1 = require("@azure/msal-react");
var react_query_1 = require("react-query");
var apiBroker_terminologyArtifacts_1 = require("../../brokers/apiBroker.terminologyArtifacts");
exports.terminologyArtifactService = {
    useCreateTerminologyArtifact: function () {
        var terminologyArtifactBroker = new apiBroker_terminologyArtifacts_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        var msal = (0, msal_react_1.useMsal)();
        return (0, react_query_1.useMutation)(function (terminologyArtifact) {
            var date = new Date();
            terminologyArtifact.createdDate = terminologyArtifact.updatedDate = date;
            terminologyArtifact.createdBy = terminologyArtifact.updatedBy = msal.accounts[0].username;
            return terminologyArtifactBroker.PostTerminologyArtifactAsync(terminologyArtifact);
        }, {
            onSuccess: function (variables) {
                queryClient.invalidateQueries("TerminologyArtifactGetAll");
                queryClient.invalidateQueries("TerminologyArtifactsGetAll");
                queryClient.invalidateQueries(["TerminologyArtifactGetById", { id: variables.id }]);
            }
        });
    },
    useGetAllTerminologyArtifacts: function (query) {
        var terminologyArtifactBroker = new apiBroker_terminologyArtifacts_1.default();
        return (0, react_query_1.useQuery)(["TerminologyArtifactGetAll", { query: query }], function () { return terminologyArtifactBroker.GetAllTerminologyArtifactsAsync(query); }, { staleTime: Infinity });
    },
    useGetAllTerminologyArtifactsPages: function (query) {
        var terminologyArtifactBroker = new apiBroker_terminologyArtifacts_1.default();
        return (0, react_query_1.useInfiniteQuery)(["TerminologyArtifactsGetAll", { query: query }], function (_a) {
            var pageParam = _a.pageParam;
            if (!pageParam) {
                return terminologyArtifactBroker.GetTerminologyArtifactsFirstPagesAsync(query);
            }
            return terminologyArtifactBroker.GetTerminologyArtifactsSubsequentPagesAsync(pageParam);
        }, {
            getNextPageParam: function (lastPage) { return lastPage.nextPage; },
            staleTime: Infinity
        });
    },
    useGetTerminologyArtifactById: function (id) {
        var terminologyArtifactBroker = new apiBroker_terminologyArtifacts_1.default();
        return (0, react_query_1.useQuery)(["TerminologyArtifactGetById", { id: id }], function () { return terminologyArtifactBroker.GetTerminologyArtifactByIdAsync(id); }, { staleTime: Infinity });
    },
    useUpdateTerminologyArtifact: function () {
        var terminologyArtifactBroker = new apiBroker_terminologyArtifacts_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        var msal = (0, msal_react_1.useMsal)();
        return (0, react_query_1.useMutation)(function (terminologyArtifact) {
            var date = new Date();
            terminologyArtifact.updatedDate = date;
            terminologyArtifact.updatedBy = msal.accounts[0].username;
            return terminologyArtifactBroker.PutTerminologyArtifactAsync(terminologyArtifact);
        }, {
            onSuccess: function (data) {
                queryClient.invalidateQueries("TerminologyArtifactGetAll");
                queryClient.invalidateQueries(["TerminologyArtifactGetById", { id: data.id }]);
            }
        });
    },
    useDeleteTerminologyArtifact: function () {
        var terminologyArtifactBroker = new apiBroker_terminologyArtifacts_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        return (0, react_query_1.useMutation)(function (id) {
            return terminologyArtifactBroker.DeleteTerminologyArtifactByIdAsync(id);
        }, {
            onSuccess: function (data) {
                queryClient.invalidateQueries("TerminologyArtifactGetAll");
                queryClient.invalidateQueries(["TerminologyArtifactGetById", { id: data.id }]);
            }
        });
    },
};
//# sourceMappingURL=terminologyArtifactService.js.map