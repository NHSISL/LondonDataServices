import { debounce } from "lodash";
import React, { FunctionComponent, useEffect, useMemo, useState } from "react";
import { optOutViewService } from "../../services/views/OptOuts/optoutViewService";
import SearchBase from "../bases/inputs/SearchBase";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import OptOutDetailCard from "./optOutDetailCard";

interface OptOutDetailProps {
    children?: React.ReactNode;
}

const OptOutDetail: FunctionComponent<OptOutDetailProps> = (props) => {
    const {
        children
    } = props;

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const { mappedOptOuts: optOutsRetrieved, isLoading } = optOutViewService.useGetAllOptOuts(debouncedTerm);

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

    return (
        <div>
            <div>
                <div className="filter-container">
                    <div className="filter-item">
                        <SearchBase
                            id="search"
                            label="Search NHS Number"
                            value={searchTerm}
                            onChange={(e) => {
                                handleSearchChange(e.currentTarget.value);
                            }}
                        />
                        {isLoading && <> <SpinnerBase />.</>}
                    </div>
                </div>

                <OptOutDetailCard
                    optOuts={ optOutsRetrieved }
                >
                    {children}
                </OptOutDetailCard>
            </div>
        </div>
    );
}
export default OptOutDetail;