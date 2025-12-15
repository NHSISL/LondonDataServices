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
import { Row, Form } from "react-bootstrap";
import { subscriberCredentialViewService } from "../../services/views/subscriberCredentials/subscriberCredentialViewService";
import { supplierViewService } from "../../services/views/suppliers/supplierViewService";
import { SubscriberCredentialView } from "../../models/views/components/subscriberCredentials/subscriberCredentialView";

type SubscriberAgreementTableProps = {};

const SubscriberAgreementTable: FunctionComponent<SubscriberAgreementTableProps> = (props) => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [showSpinner, setShowSpinner] = useState(false);
    const [selectedSupplierId, setSelectedSupplierId] = useState<string>("");
    const [activeFilter, setActiveFilter] = useState<string>("all");

    // Get suppliers for filter dropdown
    const { mappedSuppliers: suppliersRetrieved, isLoading: isSuppliersLoading } =
        supplierViewService.useGetAllSuppliers();

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
        return !isLoading && data?.pages.at(-1)?.nextPage === undefined;
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

    // Filter subscriber credentials by selected supplier and active status
    const filteredSubscriberCredentials = subscriberCredentialRetrieved
        ?.filter((cred: SubscriberCredentialView) =>
            (!selectedSupplierId || cred.supplierId?.toString() === selectedSupplierId) &&
            (activeFilter === "all" ||
                (activeFilter === "active" && cred.isActive) ||
                (activeFilter === "inactive" && !cred.isActive))
        );

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

                                    <Form.Select
                                        value={selectedSupplierId}
                                        onChange={e => setSelectedSupplierId(e.target.value)}
                                        disabled={isSuppliersLoading}
                                        style={{ maxWidth: 150, marginLeft: 10 }}
                                    >
                                        {isSuppliersLoading ? (
                                            <option value="">Loading suppliers...</option>
                                        ) : (
                                            <>
                                                <option value="">All Suppliers</option>
                                                {suppliersRetrieved &&
                                                    suppliersRetrieved
                                                        .filter((supplier: any) => supplier.isIngestionTracked === true || supplier.isIngestionTracked === 1)
                                                        .map((supplier: any) => (
                                                            <option key={supplier.id} value={supplier.id}>
                                                                {supplier.name}
                                                            </option>
                                                        ))}
                                            </>
                                        )}
                                    </Form.Select>

                                    {/* Active/Inactive Filter Dropdown */}
                                    <Form.Select
                                        value={activeFilter}
                                        onChange={e => setActiveFilter(e.target.value)}
                                        style={{ maxWidth: 150, marginLeft: 10 }}
                                    >
                                        <option value="all">All Statuses</option>
                                        <option value="active">Active</option>
                                        <option value="inactive">Inactive</option>
                                    </Form.Select>

                                    <div className="input-group-append" style={{ marginLeft: 10 }}>
                                        <Link to="/subscriberAgreement/new">
                                            <button onClick={() => { }} className="btn btn-primary"><FontAwesomeIcon icon={faPlusCircle} />&nbsp;New</button>
                                        </Link>
                                    </div>

                                    {showSpinner ? (
                                        <SpinnerBase />
                                    ) : (
                                            <div className="input-group-append" style={{ marginLeft: 10 }}>
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
                                            <TableBaseHeader>Supplier</TableBaseHeader>
                                            <TableBaseHeader>Supplier Agreement Id</TableBaseHeader>
                                            <TableBaseHeader>Sharing Agreement Name</TableBaseHeader>
                                            <TableBaseHeader>Sharing Agreement Guid</TableBaseHeader>
                                            <TableBaseHeader>Active</TableBaseHeader>
                                            <TableBaseHeader></TableBaseHeader>
                                        </TableBaseRow>
                                    </TableBaseThead>
                                    <TableBaseTbody>
                                        {filteredSubscriberCredentials?.map(
                                            (subscriberCredentialHomeView: SubscriberCredentialView) => (
                                                <SubscriberAgreementRow
                                                    key={subscriberCredentialHomeView.id.toString()}
                                                    subscriberCredential={subscriberCredentialHomeView}
                                                    suppliers={suppliersRetrieved}
                                                />)
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
                            )}
                        </InfiniteScroll>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div>
    );
};

export default SubscriberAgreementTable;