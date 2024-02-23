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
exports.subscriberAgreementViewService = void 0;
var react_1 = require("react");
var subscriberAgreement_1 = require("../../../models/views/components/subscriberAgreements/subscriberAgreement");
var subscriberAgreementService_1 = require("../../foundations/subscriberAgreementService");
exports.subscriberAgreementViewService = {
    useCreateSubscriberAgreement: function () {
        return subscriberAgreementService_1.subscriberAgreementService.useCreateSubscriberAgreement();
    },
    useGetAllSubscriberAgreements: function (searchTerm) {
        try {
            var query = "?$orderby=createdDate desc";
            if (searchTerm) {
                query = query + "&$filter=contains(supplierSharingAgreementShortName,'".concat(searchTerm, "')");
            }
            var response_1 = subscriberAgreementService_1.subscriberAgreementService.useRetrieveAllSubscriberAgreementPages(query);
            var _a = (0, react_1.useState)(), mappedSubscriberAgreements = _a[0], setMappedSubscriberAgreements_1 = _a[1];
            var _b = (0, react_1.useState)([]), pages = _b[0], setPages_1 = _b[1];
            (0, react_1.useEffect)(function () {
                if (response_1.data && response_1.data.pages) {
                    var subscriberAgreements_1 = [];
                    response_1.data.pages.forEach(function (x) {
                        x.data.forEach(function (subscriberAgreement) {
                            subscriberAgreements_1.push(new subscriberAgreement_1.SubscriberAgreementView(subscriberAgreement.id, subscriberAgreement.supplierSharingAgreementShortName, subscriberAgreement.ftpUserName, subscriberAgreement.ftpPublicKey, subscriberAgreement.gpgPublicKey, subscriberAgreement.isActive, subscriberAgreement.supplierSharingAgreementGuid, subscriberAgreement.lastPollStartDate, subscriberAgreement.lastPollEndDate, subscriberAgreement.createdBy, subscriberAgreement.createdDate, subscriberAgreement.updatedBy, subscriberAgreement.updatedDate));
                        });
                    });
                    setMappedSubscriberAgreements_1(subscriberAgreements_1);
                    setPages_1(response_1.data.pages);
                }
            }, [response_1.data]);
            return {
                mappedSubscriberAgreements: mappedSubscriberAgreements,
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
    useGetSubscriberAgreementById: function (id) {
        try {
            var query = "?$filter=id eq ".concat(id);
            var response_2 = subscriberAgreementService_1.subscriberAgreementService.useRetrieveAllSubscriberAgreement(query);
            var _a = (0, react_1.useState)(), mappedSubscriberAgreement = _a[0], setMappedSubscriberAgreement_1 = _a[1];
            (0, react_1.useEffect)(function () {
                if (response_2.data && response_2.data[0]) {
                    var subscriberAgreement = new subscriberAgreement_1.SubscriberAgreementView(response_2.data[0].id, response_2.data[0].supplierSharingAgreementShortName, response_2.data[0].ftpUserName, response_2.data[0].ftpPublicKey, response_2.data[0].gpgPublicKey, response_2.data[0].isActive, response_2.data[0].supplierSharingAgreementGuid, response_2.data[0].lastPollStartDate, response_2.data[0].lastPollEndDate, response_2.data[0].createdBy, response_2.data[0].createdDate, response_2.data[0].updatedBy, response_2.data[0].subscriberAgreement);
                    setMappedSubscriberAgreement_1(subscriberAgreement);
                }
            }, [response_2.data]);
            return __assign({ mappedSubscriberAgreement: mappedSubscriberAgreement }, response_2);
        }
        catch (err) {
            throw err;
        }
    },
    useUpdateSubscriberAgreement: function () {
        return subscriberAgreementService_1.subscriberAgreementService.useModifySubscriberAgreement();
    },
    useRemoveSubscriberAgreement: function () {
        return subscriberAgreementService_1.subscriberAgreementService.useRemoveSubscriberAgreement();
    },
};
//# sourceMappingURL=subscriberAgreementViewService.js.map