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
exports.auditViewService = void 0;
var react_1 = require("react");
var auditView_1 = require("../../models/views/components/audit/auditView");
var auditService_1 = require("../foundations/auditService");
exports.auditViewService = {
    useGetAllAudits: function (ingestionTrackingId) {
        try {
            var query = "?$orderby=createdDate";
            if (ingestionTrackingId) {
                query = query + "&$filter=ingestionTrackingId eq ".concat(ingestionTrackingId);
            }
            var response_1 = auditService_1.auditService.useGetAllAudits(query);
            var _a = (0, react_1.useState)([]), mappedAudits = _a[0], setMappedAudits_1 = _a[1];
            (0, react_1.useEffect)(function () {
                if (response_1.data) {
                    var audits = response_1.data.map(function (audit) {
                        return new auditView_1.AuditView(audit.id, audit.ingestionTrackingId, audit.message, audit.createdDate);
                    });
                    setMappedAudits_1(audits);
                }
            }, [response_1.data]);
            return __assign({ mappedAudits: mappedAudits }, response_1);
        }
        catch (err) {
            throw err;
        }
    }
};
//# sourceMappingURL=auditViewService.js.map