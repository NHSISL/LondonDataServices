import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
import PdsRow from "./pdsRow";
import CardBase from "../../bases/components/Card/CardBase";
import CardBaseBody from "../../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../../bases/components/Card/CardBase.Title";
import CardBaseContent from "../../bases/components/Card/CardBase.Content";
import InfiniteScroll from "../../bases/pagers/InfiniteScroll";
import SearchBase from "../../bases/inputs/SearchBase";
import TableBase from "../../bases/components/Table/TableBase";
import TableBaseTbody from "../../bases/components/Table/TableBase.Tbody";
import InfiniteScrollLoader from "../../bases/pagers/InfiniteScroll.Loader";
import { SpinnerBase } from "../../bases/spinner/SpinnerBase";
import { pdsHomeViewService } from "../../../services/views/pds/pdsHomeViewService";
import { PdsHomeView } from "../../../models/pds/pdsHomeView";
import TableBaseThead from "../../bases/components/Table/TableBase.Thead";
import TableBaseRow from "../../bases/components/Table/TableBase.Row";
import TableBaseHeader from "../../bases/components/Table/TableBase.Header";
import { Row } from "react-bootstrap";

type PdsTableProps = {};

const PdsTable: FunctionComponent<PdsTableProps> = (props) => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");

    const {
        mappedPds: pdsRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
    } = pdsHomeViewService.useGetAllPds(
        debouncedTerm
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

    return (
        <div className="infiniteScrollContainer">
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>Patient Demographic Search</CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll loading={isLoading} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
                            <Row>
                                <div className="filter-item">
                                    <SearchBase
                                        id="search"
                                        label="Search Pds"
                                        placeholder="search for patient file..."
                                        value={searchTerm}
                                        onChange={(e) => {
                                            handleSearchChange(e.currentTarget.value);
                                        }} />
                                </div>

                            <TableBase>
                                <TableBaseThead>
                                    <TableBaseRow>
                                        <TableBaseHeader>Status</TableBaseHeader>
                                        <TableBaseHeader>Filename</TableBaseHeader>
                                        <TableBaseHeader>Message Id</TableBaseHeader>
                                        <TableBaseHeader>Audit</TableBaseHeader>
                                    </TableBaseRow>
                                </TableBaseThead>
                                <TableBaseTbody>
                                    {pdsRetrieved?.map(
                                        (pdsHomeView: PdsHomeView) => (
                                            <PdsRow
                                                key={pdsHomeView.id.toString()}
                                                pds={pdsHomeView} />)
                                    )}
                                    <tr>
                                        <td colSpan={4} className="text-center">
                                            <InfiniteScrollLoader
                                                loading={isLoading || isFetchingNextPage}
                                                spinner={<SpinnerBase />}
                                                noMorePages={hasNoMorePages()}
                                                noMorePagesMessage={<>---No more Pds---</>} />
                                        </td>
                                    </tr>
                                </TableBaseTbody>
                                </TableBase>
                                </Row>
                        </InfiniteScroll>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
};

export default PdsTable;