import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
import SubscriberAgreementRow from "./subscriberAgreementRow";
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
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlusCircle, faRefresh } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import { Row } from "react-bootstrap";
import { subscriberCredentialViewService } from "../../services/views/subscriberCredentials/subscriberCredentialViewService";
import { SubscriberCredentialView } from "../../models/views/components/subscriberCredentials/subscriberCredentialView";

const SubscriberAgreementTable: FunctionComponent = () => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [showSpinner, setShowSpinner] = useState(false);

    const {
        mappedSubscriberCredentials: subscriberCredentialRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
        refetch
    } = subscriberCredentialViewService.useGetAllSubscriberCredentials(
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
        return !isLoading && !hasNextPage;
    };

    const refreshData = () => {
        setShowSpinner(true);
        setTimeout(() => {
            refetch();
            setTimeout(() => {
                setShowSpinner(false);
            }, 1000);
        }, 200);
    };

    return (
        <div className="infiniteScrollContainer">
            <CardBase>
                <CardBaseBody>
                    <CardBaseTitle>
                        Subscriber Agreements
                    </CardBaseTitle>
                    <CardBaseContent>
                        <InfiniteScroll loading={isLoading} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>
                            <Row>
                                <div className="input-group mb-3">
                                    <SearchBase
                                        id="search"
                                        value={searchTerm}
                                        placeholder="Search for Subscriber Agreements...."
                                        onChange={(e) => { handleSearchChange(e.currentTarget.value) }} />

                                    <div className="input-group-append">
                                        <Link to="/subscriberAgreement/new">
                                            <button onClick={() => { }} className="btn btn-primary"><FontAwesomeIcon icon={faPlusCircle} />&nbsp;New</button>
                                        </Link>
                                    </div>

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
                            </Row>

                            {!showSpinner && (
                                <TableBase>
                                    <TableBaseThead>
                                        <TableBaseRow>
                                            <TableBaseHeader>Supplier Agreement Id</TableBaseHeader>
                                            <TableBaseHeader>Supplier Sharing Agreement ShortName</TableBaseHeader>
                                            <TableBaseHeader>Supplier Sharing Agreement Guid</TableBaseHeader>
                                            <TableBaseHeader></TableBaseHeader>
                                        </TableBaseRow>
                                    </TableBaseThead>
                                    <TableBaseTbody>
                                        {subscriberCredentialRetrieved?.map(
                                            (subscriberCredentialHomeView: SubscriberCredentialView) => (
                                                <SubscriberAgreementRow
                                                    key={subscriberCredentialHomeView.id.toString()}
                                                    subscriberCredential={subscriberCredentialHomeView} />)
                                        )}
                                        <tr>
                                            <td colSpan={4} className="text-center">
                                                <InfiniteScrollLoader
                                                    loading={isLoading || isFetchingNextPage}
                                                    spinner={<SpinnerBase />}
                                                    noMorePages={!hasNoMorePages()}
                                                    noMorePagesMessage={<>---No more Subscriber Agreements---</>}
                                                    totalPages={data?.pages.length} />
                                            </td>
                                        </tr>
                                    </TableBaseTbody>
                                </TableBase>
                            )}
                        </InfiniteScroll>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
};

export default SubscriberAgreementTable;