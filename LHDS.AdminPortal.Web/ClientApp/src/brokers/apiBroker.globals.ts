import { MutationCache, QueryCache, QueryClient } from "react-query";

export const queryClientGlobalOptions = new QueryClient({
    defaultOptions: {
        queries: {
            retry: false
        }
    },
    queryCache: new QueryCache({
        onError: () => {
            //toastError("An unknown error has occured, please refresh the page and try again.");
        }
    },
    ),
    mutationCache: new MutationCache({
        onSuccess: () => {
            //toastSuccess("Saved.")
        },
        onError: (error: any) => {
            //if (!error?.response?.data?.errors) {
            //    toastError("An unknown error has occured, please try again.");
            //} else {
            //    toastWarning("Your record has not been saved, please correct and try again.");
            //    throw new ApiValidationError(error?.response?.data?.errors);
            //}
        }
    })
});