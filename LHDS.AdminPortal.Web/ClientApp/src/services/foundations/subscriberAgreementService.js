"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.subscriberAgreementService = void 0;
var msal_react_1 = require("@azure/msal-react");
var react_query_1 = require("react-query");
var apiBroker_subscriberagreement_1 = require("../../brokers/apiBroker.subscriberagreement");
exports.subscriberAgreementService = {
    useCreateSubscriberAgreement: function () {
        var broker = new apiBroker_subscriberagreement_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        var msal = (0, msal_react_1.useMsal)();
        return (0, react_query_1.useMutation)(function (subscriberAgreement) {
            var date = new Date();
            subscriberAgreement.createdDate = subscriberAgreement.updatedDate = date;
            subscriberAgreement.createdBy = subscriberAgreement.updatedBy = msal.accounts[0].username;
            return broker.PostSubscriberAgreementAsync(subscriberAgreement);
        }, {
            onSuccess: function (variables) {
                queryClient.invalidateQueries("SubscriberAgreementGetAll");
                queryClient.invalidateQueries(["SubscriberAgreementGetById", { id: variables.id }]);
            }
        });
    },
    useRetrieveAllSubscriberAgreement: function (query) {
        var broker = new apiBroker_subscriberagreement_1.default();
        return (0, react_query_1.useQuery)(["SubscriberAgreementGetAll", { query: query }], function () { return broker.GetAllSubscriberAgreementsAsync(query); }, { staleTime: Infinity });
    },
    useRetrieveAllSubscriberAgreementPages: function (query) {
        var broker = new apiBroker_subscriberagreement_1.default();
        return (0, react_query_1.useInfiniteQuery)(["SubscriberAgreementGetAll", { query: query }], function (_a) {
            var pageParam = _a.pageParam;
            if (!pageParam) {
                return broker.GetSubscriberAgreementFirstPagesAsync(query);
            }
            return broker.GetSubscriberAgreementSubsequentPagesAsync(pageParam);
        }, {
            getNextPageParam: function (lastPage) { return lastPage.nextPage; },
            staleTime: Infinity
        });
    },
    useModifySubscriberAgreement: function () {
        var broker = new apiBroker_subscriberagreement_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        var msal = (0, msal_react_1.useMsal)();
        return (0, react_query_1.useMutation)(function (subscriberAgreement) {
            var date = new Date();
            subscriberAgreement.updatedDate = date;
            subscriberAgreement.updatedBy = msal.accounts[0].username;
            return broker.PutSubscriberAgreementAsync(subscriberAgreement);
        }, {
            onSuccess: function (data) {
                queryClient.invalidateQueries("SubscriberAgreementGetAll");
                queryClient.invalidateQueries(["SubscriberAgreementGetById", { id: data.id }]);
            }
        });
    },
    useRemoveSubscriberAgreement: function () {
        var broker = new apiBroker_subscriberagreement_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        return (0, react_query_1.useMutation)(function (id) {
            return broker.DeleteSubscriberAgreementByIdAsync(id);
        }, {
            onSuccess: function (data) {
                queryClient.invalidateQueries("SubscriberAgreementGetAll");
                queryClient.invalidateQueries(["SubscriberAgreementGetById", { id: data.id }]);
            }
        });
    },
};
//# sourceMappingURL=subscriberAgreementService.js.map