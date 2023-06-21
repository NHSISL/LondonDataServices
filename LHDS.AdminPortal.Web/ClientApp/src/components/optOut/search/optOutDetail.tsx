import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState, useEffect } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import { toastSuccess } from "../../../brokers/toastBroker";
import { optOutViewService } from "../../../services/views/OptOuts/optoutViewService";
import SearchBase from "../../bases/inputs/SearchBase";
import { SpinnerBase } from "../../bases/spinner/SpinnerBase";
import OptOutDetailCard from "./optOutDetailCard";

interface OptOutDetailProps {
    children?: React.ReactNode;
}

const OptOutDetail: FunctionComponent<OptOutDetailProps> = (props) => {
    const { children } = props;

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [optOutRetrieved, setOptOutRetrieved] = useState<OptOutView | undefined>(undefined); // Initialize with undefined
    const [isLoading, setIsLoading] = useState<boolean>(false); // Add loading state

    const handleDebounce = useMemo(
        () =>
            debounce((value: string) => {
                setDebouncedTerm(value);
            }, 500),
        []
    );

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    };

    const updateOptOut = optOutViewService.useUpdateSupplier();
    const handleClearCache = async (optOutView: OptOutView) => {
        toastSuccess("Clearing Cache");
        optOutView.cacheTime = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
        return updateOptOut.mutateAsync(optOutView);
    };

    useEffect(() => {
        let cancelRequest = false;

        const fetchData = async () => {
            if (debouncedTerm && debouncedTerm.length === 10) {
                setIsLoading(true);

                try {
                    const optOutsResult = await optOutViewService.useGetOptOutsByNhsNumber(debouncedTerm);

                    if (!cancelRequest && optOutsResult.isSuccess) {
                        const optOuts = optOutsResult.data?.mappedOptOut;
                        if (optOuts) {
                            setOptOutRetrieved(optOuts);
                        }
                    }

                    setIsLoading(false);
                } catch (error) {
                    console.error("Error fetching opt outs:", error);
                    setIsLoading(false);
                }
            }
        };

        fetchData();

        return () => {
            cancelRequest = true;
        };
    }, [debouncedTerm]);

    return (
        <div>
            <div>
                <div className="filter-container">
                    <div className="filter-item">
                        <SearchBase
                            id="search"
                            label="Search NHS Number"
                            placeholder="  Search NHS Number"
                            value={searchTerm}
                            onChange={(e) => {
                                handleSearchChange(e.currentTarget.value);
                            }}
                        />
                        {isLoading && (
                            <>
                                <SpinnerBase />.
                            </>
                        )}
                    </div>
                </div>

                <OptOutDetailCard
                    optOuts={optOutRetrieved}
                    onClearCache={handleClearCache}
                >
                    {children}
                </OptOutDetailCard>
            </div>
        </div>
    );
};

export default OptOutDetail;