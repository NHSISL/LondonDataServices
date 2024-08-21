import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
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
import { Form, Row } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDatabase, faRefresh } from "@fortawesome/free-solid-svg-icons";
import TerminologyArtifactRow from "./terminologyArtifactRow";
import { TerminologyArtifact } from "../../models/terminologyArtifacts/terminologyArtifact";
import { TerminologyArtifactHomeViewService } from "../../services/views/terminologyArtifacts/terminologyArtifactHomeViewService";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseThead from "../bases/components/Table/TableBase.Thead";
import TableBaseHeader from "../bases/components/Table/TableBase.Header";

const TerminologyArtifactTable: FunctionComponent = () => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [showSpinner, setShowSpinner] = useState(false);
    const [selectedResourceType, setSelectedResourceType] = useState<string>("all");

    const {
        mappedTerminologyArtifacts: terminologyArtifactsRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
        refetch
    } = TerminologyArtifactHomeViewService.useGetAllTerminologyArtifacts(debouncedTerm, selectedResourceType);

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    };

    const handleDebounce = useMemo(
        () =>
            debounce((value: string) => {
                setDebouncedTerm(value);
            }, 500),
        []
    );

    const handleResourceTypeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newResourceType = event.target.id;
        setSelectedResourceType(newResourceType);
        handleFilter(newResourceType);
    };

    const handleResourceDebounce = useMemo(
        () =>
            debounce((value: string) => {
                setSelectedResourceType(value);
            }, 500),
        []
    );

    const handleFilter = (resourceType: string) => {
        handleResourceDebounce(resourceType);
    };

    const refreshData = () => {
        setShowSpinner(true);
        setTimeout(() => {
            refetch();
            setTimeout(() => {
                setShowSpinner(false);
            }, 200);
        }, 200);
    };

    return (
        <div className="infiniteScrollContainer">
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        <FontAwesomeIcon icon={faDatabase} className="me-2" /> Terminology Artifacts
                    </CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll loading={isLoading || showSpinner} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
                            <Row>
                                <div className="input-group mb-3">
                                    <SearchBase
                                        id="search"
                                        value={searchTerm}
                                        placeholder="Search for terminology artifacts..."
                                        onChange={(e) => { handleSearchChange(e.currentTarget.value) }} />

                                    {showSpinner ? (
                                        <SpinnerBase />
                                    ) : (
                                        <div className="input-group-append">
                                            <button className="btn btn-outline-secondary" id="refreshButton" onClick={refreshData}>
                                                <FontAwesomeIcon icon={faRefresh} /> Refresh
                                            </button>
                                        </div>
                                    )}
                                </div>
                                <Row>
                                    <span className="input-group mb-3">
                                        <div className="input-group mb-3">
                                            <Form.Check
                                                type="radio"
                                                id="all"
                                                name="resourceType"
                                                label="All"
                                                onChange={handleResourceTypeChange}
                                                checked={selectedResourceType === "all"}
                                            />
                                            <Form.Check
                                                type="radio"
                                                id="codeSystem"
                                                name="resourceType"
                                                label="CodeSystem"
                                                className="ms-3"
                                                onChange={handleResourceTypeChange}
                                                checked={selectedResourceType === "codeSystem"}
                                            />
                                            <Form.Check
                                                type="radio"
                                                id="valueSet"
                                                name="resourceType"
                                                label="ValueSet"
                                                className="ms-3"
                                                onChange={handleResourceTypeChange}
                                                checked={selectedResourceType === "valueSet"}
                                            />
                                            <Form.Check
                                                type="radio"
                                                id="conceptMap"
                                                name="resourceType"
                                                label="ConceptMap"
                                                className="ms-3"
                                                onChange={handleResourceTypeChange}
                                                checked={selectedResourceType === "conceptMap"}
                                            />
                                        </div>
                                    </span>
                                </Row>
                            </Row>
                            <TableBase classes="table-bordered">
                                <TableBaseThead>
                                    <TableBaseRow>
                                        <TableBaseHeader>Resource Type</TableBaseHeader>
                                        <TableBaseHeader>Name</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Version</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Status</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Error</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Marked as</TableBaseHeader>
                                        <TableBaseHeader classes="text-center">Downloaded</TableBaseHeader>
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
                                            {terminologyArtifactsRetrieved?.map(
                                                (terminologyArtifact: TerminologyArtifact) => (
                                                    <TerminologyArtifactRow
                                                        key={terminologyArtifact.id.toString()}
                                                        terminologyArtifact={terminologyArtifact}
                                                    />
                                                )
                                            )}
                                            <tr>
                                                <td colSpan={5} className="text-center">
                                                    <InfiniteScrollLoader
                                                        loading={isLoading || isFetchingNextPage}
                                                        spinner={<SpinnerBase />}
                                                        noMorePages={!hasNextPage}
                                                        noMorePagesMessage={<>-- No more pages --</>}
                                                        totalPages={data?.pages.length} />
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
        </div>
    );
};

export default TerminologyArtifactTable;
