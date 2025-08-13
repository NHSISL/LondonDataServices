import { debounce } from "lodash";
import React, { FunctionComponent, useEffect, useMemo, useState } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";
import SearchBase from "../bases/inputs/SearchBase";
import InfiniteScroll from "../bases/pagers/InfiniteScroll";
import InfiniteScrollLoader from "../bases/pagers/InfiniteScroll.Loader";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import { Button, Form, Row } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDatabase, faFilter, faRefresh, faTimes } from "@fortawesome/free-solid-svg-icons";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseThead from "../bases/components/Table/TableBase.Thead";
import TableBaseHeader from "../bases/components/Table/TableBase.Header";
import { ResolvedAddressHomeViewService } from "../../services/views/resolvedAddresses/resolvedAddressHomeViewService";
import { ResolvedAddress } from "../../models/resolvedAddresses/resolvedAddress";
import ResolvedAddressRow from "./resolvedAddressRow";

const ResolvedAddressTable: FunctionComponent = () => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [showSpinner, setShowSpinner] = useState(false);
    const [selectedMatchedFilter, setSelectedMatchedFilter] = useState<boolean | null>(null);
    const [showSidePanel, setShowSidePanel] = useState(false);

    const selectedIsMatched: boolean | undefined = selectedMatchedFilter === null ? undefined : selectedMatchedFilter;

    const {
        mappedResolvedAddresses: resolvedAddressesRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
        refetch
    } = ResolvedAddressHomeViewService.useGetAllResolvedAddresses(debouncedTerm, selectedIsMatched);

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    };

    const isFilterActive = selectedIsMatched !== undefined;

    const handleDebounce = useMemo(
        () =>
            debounce((value: string) => {
                setDebouncedTerm(value);
            }, 500),
        []
    );

    useEffect(() => {
        return () => {
            handleDebounce.cancel();
        };
    }, [handleDebounce]);

    const refreshData = () => {
        setShowSpinner(true);
        setTimeout(() => {
            refetch();
            setTimeout(() => {
                setShowSpinner(false);
            }, 200);
        }, 200);
    };

    const handleClearFilters = () => {
        setSelectedMatchedFilter(null);
        setShowSidePanel(false);
        refetch();
    };

    return (
        <div className="infiniteScrollContainer">
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        <FontAwesomeIcon icon={faDatabase} className="me-2" /> Resolved Addresses
                    </CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll loading={isLoading || showSpinner} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
                            <Row className="m-0 p-0">
                                <div className="input-group mb-3 m-0 p-0">
                                    <SearchBase
                                        id="search"
                                        value={searchTerm}
                                        placeholder="Search for Resolved Addresses..."
                                        onChange={(e) => handleSearchChange(e.currentTarget.value)}
                                    />

                                    <div className="btn-group" role="group" aria-label="Filter and Refresh buttons">
                                        <button
                                            className={`btn btn-outline-secondary ${isFilterActive ? "btn-outline-danger" : ""}`}
                                            id="filterButton"
                                            onClick={() => setShowSidePanel((prev) => !prev)}
                                        >
                                            <FontAwesomeIcon icon={faFilter} />
                                        </button>
                                        {showSpinner ? (
                                            <SpinnerBase />
                                        ) : (
                                            <div className="input-group-append">
                                                <button className="btn btn-outline-secondary" id="refreshButton" onClick={refreshData}>
                                                    <FontAwesomeIcon icon={faRefresh} />
                                                </button>
                                            </div>
                                        )}
                                    </div>
                                </div>
                            </Row>
                            <TableBase classes="table-bordered">
                                <TableBaseThead>
                                    <TableBaseRow>
                                        <TableBaseHeader>UPRN</TableBaseHeader>
                                        <TableBaseHeader>USRN</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Unstructured Address</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Matched Address</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Matched</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Actions</TableBaseHeader>
                                    </TableBaseRow>
                                </TableBaseThead>

                                <TableBaseTbody>
                                    {isLoading || showSpinner ? (
                                        <tr>
                                            <td colSpan={6} className="text-center">
                                                <SpinnerBase />
                                            </td>
                                        </tr>
                                    ) : (
                                        <>
                                            {resolvedAddressesRetrieved?.map(
                                                (resolvedAddress: ResolvedAddress) => (
                                                    <ResolvedAddressRow
                                                        key={resolvedAddress.id.toString()}
                                                        resolvedAddress={resolvedAddress}
                                                    />
                                                )
                                            )}
                                            <tr>
                                                <td colSpan={6} className="text-center">
                                                    <InfiniteScrollLoader
                                                        loading={isLoading || isFetchingNextPage}
                                                        spinner={<SpinnerBase />}
                                                        noMorePages={!hasNextPage}
                                                        noMorePagesMessage={<>-- No more pages --</>}
                                                        totalPages={data?.pages.length}
                                                    />
                                                </td>
                                            </tr>
                                        </>
                                    )}
                                </TableBaseTbody>
                            </TableBase>
                        </InfiniteScroll>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>

            <div className={`filter-side-panel ${showSidePanel ? "open" : ""}`}>
                <div className="filter-side-panel-header">
                    <h5><FontAwesomeIcon icon={faFilter} /> Filters</h5>
                    <button className="close-button" onClick={() => setShowSidePanel(false)}>×</button>
                </div>
                <div className="filter-side-panel-body">
                    <Form.Group>
                        <Form.Label className="fw-semibold">Is Matched Filter</Form.Label>
                        <div className="d-flex gap-3 mt-2">
                            <Form.Check
                                inline
                                label="All"
                                name="matchedFilter"
                                type="radio"
                                id="matched-all"
                                value="all"
                                checked={selectedMatchedFilter === null}
                                onChange={() => setSelectedMatchedFilter(null)}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="Matched"
                                name="matchedFilter"
                                type="radio"
                                id="matched-yes"
                                value="true"
                                checked={selectedMatchedFilter === true}
                                onChange={() => setSelectedMatchedFilter(true)}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="Not Matched"
                                name="matchedFilter"
                                type="radio"
                                id="matched-no"
                                value="false"
                                checked={selectedMatchedFilter === false}
                                onChange={() => setSelectedMatchedFilter(false)}
                                className="cursor-pointer"
                            />
                        </div>

                        <div className="d-flex justify-content-between align-items-center mt-auto">
                            <Button
                                variant="outline-secondary"
                                onClick={handleClearFilters}
                                className="fw-semibold"
                                style={{ minWidth: 110 }}
                            >
                                <FontAwesomeIcon icon={faTimes} /> Clear
                            </Button>
                            
                        </div>
                    </Form.Group>
                </div>
            </div>
        </div>
    );
};

export default ResolvedAddressTable;
