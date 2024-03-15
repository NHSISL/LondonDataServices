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
exports.subscriberCredentialViewService = void 0;
var react_1 = require("react");
var subscriberCredentialView_1 = require("../../../models/views/components/subscriberCredentials/subscriberCredentialView");
var subscriberCredentialService_1 = require("../../foundations/subscriberCredentialService");
exports.subscriberCredentialViewService = {
    useCreateSubscriberCredential: function () {
        return subscriberCredentialService_1.subscriberCredentialService.useCreateSubscriberCredential();
    },
    useGetAllSubscriberCredentials: function (searchTerm) {
        try {
            var query = "?$orderby=createdDate desc";
            if (searchTerm) {
                query = query + "&$filter=contains(supplierSharingAgreementShortName,'".concat(searchTerm, "')");
            }
            var response_1 = subscriberCredentialService_1.subscriberCredentialService.useRetrieveAllSubscriberCredentialPages(query);
            var _a = (0, react_1.useState)(), mappedSubscriberCredentials = _a[0], setMappedSubscriberCredentials_1 = _a[1];
            var _b = (0, react_1.useState)([]), pages = _b[0], setPages_1 = _b[1];
            (0, react_1.useEffect)(function () {
                if (response_1.data && response_1.data.pages) {
                    var subscriberCredentials_1 = [];
                    response_1.data.pages.forEach(function (x) {
                        x.data.forEach(function (subscriberCredential) {
                            subscriberCredentials_1.push(new subscriberCredentialView_1.SubscriberCredentialView(subscriberCredential.id, subscriberCredential.supplierSharingAgreementShortName, subscriberCredential.ftpUserName, subscriberCredential.ftpPublicKey, subscriberCredential.gpgPublicKey, subscriberCredential.isActive, subscriberCredential.supplierSharingAgreementGuid, subscriberCredential.lastPollStartDate, subscriberCredential.lastPollEndDate, subscriberCredential.createdBy, subscriberCredential.createdDate, subscriberCredential.updatedBy, subscriberCredential.updatedDate));
                        });
                    });
                    setMappedSubscriberCredentials_1(subscriberCredentials_1);
                    setPages_1(response_1.data.pages);
                }
            }, [response_1.data]);
            return {
                mappedSubscriberCredentials: mappedSubscriberCredentials,
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
    useGetSubscriberCredentialById: function (id) {
        try {
            var query = "?$filter=id eq ".concat(id);
            var response_2 = subscriberCredentialService_1.subscriberCredentialService.useRetrieveAllSubscriberCredential(query);
            var _a = (0, react_1.useState)(), mappedSubscriberCredential = _a[0], setMappedSubscriberCredential_1 = _a[1];
            (0, react_1.useEffect)(function () {
                if (response_2.data && response_2.data[0]) {
                    var subscriberCredential = new subscriberCredentialView_1.SubscriberCredentialView(response_2.data[0].id, response_2.data[0].supplierSharingAgreementShortName, response_2.data[0].ftpUserName, response_2.data[0].ftpPublicKey, response_2.data[0].gpgPublicKey, response_2.data[0].isActive, response_2.data[0].supplierSharingAgreementGuid, response_2.data[0].lastPollStartDate, response_2.data[0].lastPollEndDate, response_2.data[0].createdBy, response_2.data[0].createdDate, response_2.data[0].updatedBy, response_2.data[0].subscriberAgreement);
                    setMappedSubscriberCredential_1(subscriberCredential);
                }
            }, [response_2.data]);
            return __assign({ mappedSubscriberCredential: mappedSubscriberCredential }, response_2);
        }
        catch (err) {
            throw err;
        }
    },
    useUpdateSubscriberCredential: function () {
        return subscriberCredentialService_1.subscriberCredentialService.useModifySubscriberCredential();
    },
    useRemoveSubscriberCredential: function () {
        return subscriberCredentialService_1.subscriberCredentialService.useRemoveSubscriberCredential();
    },
};
//# sourceMappingURL=subscriberCredentialViewService.js.map