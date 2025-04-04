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
import { Button, Col, Row } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faRefresh } from "@fortawesome/free-solid-svg-icons";
import TableBaseThead from "../bases/components/Table/TableBase.Thead";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseData from "../bases/components/Table/TableBase.Data";
import { SecuredComponents } from "../links";
import { Link } from "react-router-dom";
import ButtonBase from "../bases/buttons/ButtonBase";
import securityPoints from "../../securityMatrix";
import { specificationObjectViewService } from "../../services/views/specificationObjects/specificationObjectViewService";
import { SpecificationObjectView } from "../../models/views/components/specificationObjects/specificationObjectView";
import SpecificationObjectRow from "./specificationObjectRow";

type SpecificationObjectTableProps = {
    dataSetSpecificationId: string;
    dataSetId: string;
    children?: React.ReactNode;
};

const SpecificationObjectTable: FunctionComponent<SpecificationObjectTableProps> = (props) => {
    const {
        dataSetSpecificationId,
        dataSetId
    } = props;

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [showSpinner, setShowSpinner] = useState(false);

    const {
        mappedSpecificationObjects: SpecificationObjectsRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
        refetch
    } = specificationObjectViewService.useGetAllSpecificationObjects(
        dataSetSpecificationId, debouncedTerm
    );

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

    const hasNoMorePages = () => {
        return !isLoading && data?.pages.at(-1)?.nextPage === undefined;
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
                    <CardBaseTitle>Specification Objects</CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll loading={isLoading || showSpinner} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
                            <Row>
                                <Col style={{ textAlign: "right" }}>
                                    <SecuredComponents allowedRoles={securityPoints.dataSets.add}>
                                        <>
                                            <Link to={'/configuration/SpecificationObjectAdd/' + dataSetSpecificationId + '/' + dataSetId}>
                                                <ButtonBase onClick={() => { }} add>&nbsp;Add Specification Object</ButtonBase>
                                            </Link>
                                        </>
                                    </SecuredComponents>
                                </Col>

                            </Row>
                            <Row>
                                <div className="filter-container">
                                    <div className="filter-item">
                                        <SearchBase
                                            id="search"
                                            label="Search Specification Objects"
                                            value={searchTerm}
                                            onChange={(e) => {
                                                handleSearchChange(e.currentTarget.value);
                                            }}
                                        />
                                    </div>
                                </div>
                            </Row>
                            <Row>

                                <Col style={{ textAlign: "right" }}>
                                    {showSpinner ? (
                                        <SpinnerBase />
                                    ) : (
                                        <Button variant="light">
                                            <FontAwesomeIcon icon={faRefresh} onClick={refreshData} />
                                        </Button>
                                    )}
                                </Col>
                            </Row>
                            <TableBase>
                                <TableBaseThead>
                                    <TableBaseRow>
                                        <TableBaseData><strong>Our Object Name</strong></TableBaseData>
                                        <TableBaseData><strong>Supplier Object Name</strong></TableBaseData>
                                        <TableBaseData><strong>Created By</strong></TableBaseData>
                                        <TableBaseData><strong>Created When</strong></TableBaseData>
                                        <TableBaseData classes="text-left"></TableBaseData>
                                    </TableBaseRow>
                                </TableBaseThead>
                                <TableBaseTbody>
                                    {isLoading || showSpinner ? (
                                        <tr>
                                            <td colSpan={4} className="text-center">
                                                <SpinnerBase />
                                            </td>
                                        </tr>
                                    ) : (
                                        <>
                                            {SpecificationObjectsRetrieved?.map(
                                                (specificationObjectView: SpecificationObjectView) => (
                                                    <SpecificationObjectRow
                                                        key={specificationObjectView.id.toString()}
                                                        specificationObject={specificationObjectView} />
                                                )
                                            )}
                                            <tr>
                                                <td colSpan={7} className="text-center">
                                                    <InfiniteScrollLoader
                                                        loading={isLoading || isFetchingNextPage}
                                                        spinner={<SpinnerBase />}
                                                        noMorePages={hasNoMorePages()}
                                                        noMorePagesMessage={<>-- No more pages --</>}
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
        </div>
    );
};

export default SpecificationObjectTable;