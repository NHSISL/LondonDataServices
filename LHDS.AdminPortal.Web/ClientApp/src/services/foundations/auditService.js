"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.auditService = void 0;
var react_query_1 = require("react-query");
var apiBroker_audits_1 = require("../../brokers/apiBroker.audits");
exports.auditService = {
    useGetAllAudits: function (query) {
        var auditBroker = new apiBroker_audits_1.default();
        return (0, react_query_1.useQuery)(["AuditGetAll", { query: query }], function () { return auditBroker.GetAllAuditsAsync(query); }, { staleTime: Infinity });
    }
};
//# sourceMappingURL=auditService.js.map