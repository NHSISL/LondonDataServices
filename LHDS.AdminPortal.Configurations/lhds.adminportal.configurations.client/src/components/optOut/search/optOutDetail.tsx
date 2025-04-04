import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
import { OptOutView } from "../../../models/views/components/optOuts/optOutView";
import { optOutViewService } from "../../../services/views/optOuts/optoutViewService";
import SearchBase from "../../bases/inputs/SearchBase";
import { SpinnerBase } from "../../bases/spinner/SpinnerBase";
import OptOutDetailCard from "./optOutDetailCard";
import CardBase from "../../bases/components/Card/CardBase";
import CardBaseBody from "../../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../../bases/components/Card/CardBase.Title";
import CardBaseContent from "../../bases/components/Card/CardBase.Content";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFilter } from "@fortawesome/free-solid-svg-icons";
import { Row } from "react-bootstrap";
import { Link } from "react-router-dom";
import { toastSuccess } from "../../../brokers/toastBroker.success";

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
            optOutView.id = crypto.randomUUID(),
            optOutView.nhsNumber = debouncedTerm,
            optOutView.status = "Unknown",
            optOutView.cacheTime = new Date(),
            optOutView.lastSentToMesh = new Date(),
        );

        console.log(`Adding new NHS number: ${debouncedTerm}`);
        return addNewOptOut.mutateAsync(optOutView);
    };

    return (
        <CardBase>
            <CardBaseBody>
                <CardBaseTitle>
                    Patient Opt-Out
                </CardBaseTitle>
                <CardBaseContent>
                    <Row>
                        <div className="input-group mb-3">
                            <SearchBase
                                id="search"
                                label="Search NHS Number"
                                placeholder="  Search by NHS Number"
                                value={searchTerm}
                                onChange={(e) => {
                                    handleSearchChange(e.currentTarget.value);
                                }}
                            />

                            <div className="input-group-append">
                                <Link to="/optOutUpload" className="btn btn-outline-secondary" id="filterButton">
                                    <FontAwesomeIcon icon={faFilter} /> Upload
                                </Link>
                            </div>

                            {isFetching && (
                                <>
                                    <SpinnerBase />
                                </>
                            )}
                        </div>
                    </Row>
                <OptOutDetailCard
                    optOuts={mappedOptOut}
                    onClearCache={handleClearCache}
                    onAddNewNHS={addOptOut}
                    isValidNumber={isValidNhsNumber(debouncedTerm)}
                    nhsNumber={debouncedTerm}>

                    {children}
                </OptOutDetailCard>
                </CardBaseContent>
            </CardBaseBody>
        </CardBase>
    );
};

export default OptOutDetail;