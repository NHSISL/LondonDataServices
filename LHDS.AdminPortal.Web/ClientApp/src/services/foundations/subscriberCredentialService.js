"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.subscriberCredentialService = void 0;
var msal_react_1 = require("@azure/msal-react");
var react_query_1 = require("react-query");
var apiBroker_subscriberCredentials_1 = require("../../brokers/apiBroker.subscriberCredentials");
exports.subscriberCredentialService = {
    useCreateSubscriberCredential: function () {
        var broker = new apiBroker_subscriberCredentials_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        var msal = (0, msal_react_1.useMsal)();
        return (0, react_query_1.useMutation)(function (subscriberCredential) {
            var date = new Date();
            subscriberCredential.createdDate = subscriberCredential.updatedDate = date;
            subscriberCredential.createdBy = subscriberCredential.updatedBy = msal.accounts[0].username;
            return broker.PostSubscriberCredentialAndRegenerateKeysAsync(subscriberCredential);
        }, {
            onSuccess: function (variables) {
                queryClient.invalidateQueries("SubscriberCredentialGetAll");
                queryClient.invalidateQueries(["SubscriberCredentialGetById", { id: variables.id }]);
            }
        });
    },
    useRegenerateKeysSubscriberCredential: function () {
        var broker = new apiBroker_subscriberCredentials_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        var msal = (0, msal_react_1.useMsal)();
        return (0, react_query_1.useMutation)(function (subscriberCredential) {
            var date = new Date();
            subscriberCredential.createdDate = subscriberCredential.updatedDate = date;
            subscriberCredential.createdBy = subscriberCredential.updatedBy = msal.accounts[0].username;
            return broker.PostSubscriberCredentialAndRegenerateKeysAsync(subscriberCredential);
        }, {
            onSuccess: function (variables) {
                queryClient.invalidateQueries("SubscriberCredentialGetAll");
                queryClient.invalidateQueries(["SubscriberCredentialGetById", { id: variables.id }]);
            }
        });
    },
    useRetrieveAllSubscriberCredential: function (query) {
        var broker = new apiBroker_subscriberCredentials_1.default();
        return (0, react_query_1.useQuery)(["SubscriberCredentialGetAll", { query: query }], function () { return broker.GetAllSubscriberCredentialsAsync(query); }, { staleTime: Infinity });
    },
    useRetrieveAllSubscriberCredentialPages: function (query) {
        var broker = new apiBroker_subscriberCredentials_1.default();
        return (0, react_query_1.useInfiniteQuery)(["SubscriberCredentialGetAll", { query: query }], function (_a) {
            var pageParam = _a.pageParam;
            if (!pageParam) {
                return broker.GetSubscriberCredentialFirstPagesAsync(query);
            }
            return broker.GetSubscriberCredentialSubsequentPagesAsync(pageParam);
        }, {
            getNextPageParam: function (lastPage) { return lastPage.nextPage; },
            staleTime: Infinity
        });
    },
    useModifySubscriberCredential: function () {
        var broker = new apiBroker_subscriberCredentials_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        var msal = (0, msal_react_1.useMsal)();
        return (0, react_query_1.useMutation)(function (subscriberCredential) {
            var date = new Date();
            subscriberCredential.updatedDate = date;
            subscriberCredential.updatedBy = msal.accounts[0].username;
            return broker.PutSubscriberCredentialAsync(subscriberCredential);
        }, {
            onSuccess: function (data) {
                queryClient.invalidateQueries("SubscriberCredentialGetAll");
                queryClient.invalidateQueries(["SubscriberCredentialGetById", { id: data.id }]);
            }
        });
    },
    useRemoveSubscriberCredential: function () {
        var broker = new apiBroker_subscriberCredentials_1.default();
        var queryClient = (0, react_query_1.useQueryClient)();
        return (0, react_query_1.useMutation)(function (id) {
            return broker.DeleteSubscriberCredentialByIdAsync(id);
        }, {
            onSuccess: function (data) {
                queryClient.invalidateQueries("SubscriberCredentialGetAll");
                queryClient.invalidateQueries(["SubscriberCredentialGetById", { id: data.id }]);
            }
        });
    },
};
//# sourceMappingURL=subscriberCredentialService.js.map