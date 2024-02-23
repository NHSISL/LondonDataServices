import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
import SubscriberAgreementRow from "./subscriberAgreementRow";
import { SubscriberAgreementView } from "../../models/views/components/subscriberAgreements/subscriberAgreement";
import { subscriberAgreementViewService } from "../../services/views/subscriberAgreements/subscriberAgreementViewService";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import InfiniteScroll from "../bases/pagers/InfiniteScroll";
import SearchBase from "../bases/inputs/SearchBase";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseThead from "../bases/components/Table/TableBase.Thead";
import TableBaseRow from "../bases/components/Table/TableBase.Row";
import TableBaseHeader from "../bases/components/Table/TableBase.Header";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";
import InfiniteScrollLoader from "../bases/pagers/InfiniteScroll.Loader";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import ButtonBase from "../bases/buttons/ButtonBase";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";

type SubscriberAgreementTableProps = {};

const SubscriberAgreementTable: FunctionComponent<SubscriberAgreementTableProps> = (props) => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");

    const {
        mappedSubscriberAgreements: subscriberAgreementRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
    } = subscriberAgreementViewService.useGetAllSubscriberAgreements(
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
                    <CardBaseTitle>Subscriber Agreement Search</CardBaseTitle>
                    <CardBaseContent>
                        <ButtonBase onClick={() => { }} add><FontAwesomeIcon icon={faPlusCircle} /> New</ButtonBase>
                        <InfiniteScroll loading={isLoading} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
                            <div className="filter-container">
                                <div className="filter-item">
                                    <SearchBase
                                        id="search"
                                        label="Search Subscriber Agreements"
                                        value={searchTerm}
                                        onChange={(e) => {
                                            handleSearchChange(e.currentTarget.value);
                                        }} />
                                </div>
                            </div>

                            <TableBase>
                                <TableBaseThead>
                                    <TableBaseRow>
                                        <TableBaseHeader>Col 1</TableBaseHeader>
                                        <TableBaseHeader>Col 2</TableBaseHeader>
                                    </TableBaseRow>
                                </TableBaseThead>
                                <TableBaseTbody>
                                    {subscriberAgreementRetrieved?.map(
                                        (subscriberAgreementHomeView: SubscriberAgreementView) => (
                                            <SubscriberAgreementRow
                                                key={subscriberAgreementHomeView.id.toString()}
                                                subscriberAgreement={subscriberAgreementHomeView} />)
                                    )}
                                    <tr>
                                        <td colSpan={4} className="text-center">
                                            <InfiniteScrollLoader
                                                loading={isLoading || isFetchingNextPage}
                                                spinner={<SpinnerBase />}
                                                noMorePages={hasNoMorePages()}
                                                noMorePagesMessage={<>---No more Subscriber Agreements---</>} />
                                        </td>
                                    </tr>
                                </TableBaseTbody>
                            </TableBase>
                        </InfiniteScroll>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
};

export default SubscriberAgreementTable;