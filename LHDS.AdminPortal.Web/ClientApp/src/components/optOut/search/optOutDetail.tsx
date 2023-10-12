import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import { toastSuccess } from "../../../brokers/toastBroker";
import { optOutViewService } from "../../../services/views/OptOuts/optoutViewService";
import SearchBase from "../../bases/inputs/SearchBase";
import { SpinnerBase } from "../../bases/spinner/SpinnerBase";
import OptOutDetailCard from "./optOutDetailCard";
import { Guid } from "guid-typescript";

interface OptOutDetailProps {
    children?: React.ReactNode;
}

const OptOutDetail: FunctionComponent<OptOutDetailProps> = (props) => {
    const {
        children
    } = props;

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");

    const handleDebounce = useMemo(
        () =>
            debounce((value: string) => {
                if (value) {
                    if (isValidNhsNumber(value)) {
                        setDebouncedTerm(value);
                    } else {
                        setDebouncedTerm("");
                    }
                }
            }, 500),
        []
    );

    const { mappedOptOut, isFetching } = optOutViewService.useGetOptOutsByNhsNumber(debouncedTerm);

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    };

    const updateOptOut = optOutViewService.useUpdateOptOut();
    const handleClearCache = async (optOutView: OptOutView) => {
        toastSuccess("Clearing Cache");
        optOutView.cacheTime = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000);
        return updateOptOut.mutateAsync(optOutView);
    };

    const isValidNhsNumber = (nhsNumber: string): boolean => {
        if (nhsNumber == null || nhsNumber.length !== 10) {
            return false;
        }

        const multipliers = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        let currentNumber = 0;
        let currentSum = 0;
        let currentMultiplier = 0;
        let currentString = "";
        const checkDigit = nhsNumber.substr(nhsNumber.length - 1, 1);
        const checkNumber = parseInt(checkDigit);
        let remainder = 0;
        let total = 0;

        for (let i = 0; i <= 8; i++) {
            currentString = nhsNumber.substr(i, 1);

            currentNumber = parseInt(currentString);
            currentMultiplier = multipliers[i];
            currentSum += currentNumber * currentMultiplier;
        }

        remainder = currentSum % 11;
        total = 11 - remainder;

        if (total === 11) {
            total = 0;
        }

        if (total === checkNumber) {
            return true;
        }

        return false;
    };

    const addNewOptOut = optOutViewService.useCreateOptOut();

    const addOptOut = async (optOutView: OptOutView) => {
        optOutView = new OptOutView(
            optOutView.id = Guid.create(),
            optOutView.nhsNumber = debouncedTerm,
            optOutView.status = "Unknown",
            optOutView.cacheTime = new Date(),
            optOutView.lastSentToMesh = new Date(),
        );

        console.log(`Adding new NHS number: ${debouncedTerm}`);
        return addNewOptOut.mutateAsync(optOutView);
    };

    return (
        <div>
            <div>
                <div className="filter-container mt-2">
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
                        {isFetching && (
                            <>
                                <SpinnerBase />
                            </>
                        )}
                    </div>
                </div>

                <OptOutDetailCard
                    optOuts={mappedOptOut}
                    onClearCache={handleClearCache}
                    onAddNewNHS={addOptOut}
                    isValidNumber={isValidNhsNumber(debouncedTerm)}
                    nhsNumber={debouncedTerm}>

                    {children}
                </OptOutDetailCard>
            </div>
        </div>
    );
};

export default OptOutDetail;