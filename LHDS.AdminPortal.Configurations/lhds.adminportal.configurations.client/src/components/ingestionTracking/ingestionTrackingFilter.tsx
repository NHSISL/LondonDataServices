import React, { ChangeEvent, FunctionComponent, useState } from "react";
import { Form, Button, Card } from "react-bootstrap";
import '../../styles/base.scss';
import { faFilter, faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { lookupViewService } from "../../services/views/lookups/lookupViewService";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
interface FilterModalProps {
    onAddFilter: (
        supplier: SupplierView | null,
        decryptedFilter?: boolean,
        downloadedFilter?: boolean,
        batchCompleteFilter?: boolean,
        processingFilter?: boolean) => void;

    selectedSupplierId: string;
    initialDecryptedFilter?: boolean | null;
    initialDownloadedFilter?: boolean | null;
    initialBatchCompleteFilter?: boolean | null;
    initialProcessingFilter?: boolean | null;
}

const IngestionFilterModal: FunctionComponent<FilterModalProps> = ({
    onAddFilter,
    selectedSupplierId: initialSelectedSupplierId,
    initialDecryptedFilter = null,
    initialDownloadedFilter = null,
    initialBatchCompleteFilter = null,
    initialProcessingFilter = null
}) => {
    const { mappedSuppliers: suppliersRetrieved } = lookupViewService.useGetTrackedSupplierList("");
    const [selectedSupplierId, setSelectedSupplierId] = useState<string>(initialSelectedSupplierId);
    const [selectedDecryptedFilter, setSelectedDecryptedFilter] = useState<boolean | null>(initialDecryptedFilter);
    const [selectedDownloadedFilter, setSelectedDownloadedFilter] = useState<boolean | null>(initialDownloadedFilter);
    const [selectedBatchCompleteFilter, setSelectedBatchCompleteFilter] = useState<boolean | null>(initialBatchCompleteFilter);
    const [selectedProcessingFilter, setSelectedProcessingFilter] = useState<boolean | null>(initialProcessingFilter);

    const handleSupplierChange = (event: ChangeEvent<HTMLInputElement>) => {
        setSelectedSupplierId(event.target.value);
    };

    const handleDecryptedChange = (event: ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;
        if (value === "all") setSelectedDecryptedFilter(null);
        else if (value === "true") setSelectedDecryptedFilter(true);
        else if (value === "false") setSelectedDecryptedFilter(false);
    };

    const handleDownloadedChange = (event: ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;
        if (value === "all") setSelectedDownloadedFilter(null);
        else if (value === "true") setSelectedDownloadedFilter(true);
        else if (value === "false") setSelectedDownloadedFilter(false);
    };
    const handleBatchCompleteChange = (event: ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;
        if (value === "all") setSelectedBatchCompleteFilter(null);
        else if (value === "true") setSelectedBatchCompleteFilter(true);
        else if (value === "false") setSelectedBatchCompleteFilter(false);
    };

    const handleProcessingChange = (event: ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;
        if (value === "all") setSelectedProcessingFilter(null);
        else if (value === "true") setSelectedProcessingFilter(true);
        else if (value === "false") setSelectedProcessingFilter(false);
    };


    const handleClearFilters = () => {
        setSelectedSupplierId("");
        setSelectedDecryptedFilter(null);
        setSelectedDownloadedFilter(null);
        setSelectedBatchCompleteFilter(null);
        setSelectedProcessingFilter(null);

        onAddFilter(null, undefined, undefined, undefined, undefined);
    };

    const handleFilterClick = () => {
        let supplier: SupplierView | null = null;
        if (selectedSupplierId !== "") {
            const selectedLookup = suppliersRetrieved.find(s => s.id === selectedSupplierId);
            if (selectedLookup) {
                supplier = new SupplierView(Guid.parse(selectedLookup.id));
                supplier.name = selectedLookup.name;
            }
        }

        onAddFilter(
            supplier,
            selectedDecryptedFilter === null ? undefined : selectedDecryptedFilter,
            selectedDownloadedFilter === null ? undefined : selectedDownloadedFilter,
            selectedBatchCompleteFilter === null ? undefined : selectedBatchCompleteFilter,
            selectedProcessingFilter === null ? undefined : selectedProcessingFilter
        );
    };

    return (
        <div className="d-flex flex-column h-100 shadow-md" style={{ maxWidth: 500 }}>
            <h5 className="mb-4 fw-bold text-left">Filter Ingestions</h5>

            <Card className="mb-4">
                <Card.Body>
                    <Form.Group>
                        <Form.Label className="fw-semibold">Supplier Filter</Form.Label>
                        <div className="d-flex flex-wrap gap-2 mt-2">
                            <Form.Check
                                inline
                                label="All"
                                name="supplier"
                                type="radio"
                                id="supplier-all"
                                value=""
                                checked={selectedSupplierId === ""}
                                onChange={handleSupplierChange}
                                className="cursor-pointer"
                            />
                            {suppliersRetrieved.map((supplier) => (
                                <Form.Check
                                    inline
                                    key={supplier.id}
                                    label={supplier.name}
                                    name="supplier"
                                    type="radio"
                                    id={`supplier-${supplier.id}`}
                                    value={supplier.id}
                                    checked={selectedSupplierId === supplier.id}
                                    onChange={handleSupplierChange}
                                    className="cursor-pointer"
                                />
                            ))}
                        </div>
                    </Form.Group>
                </Card.Body>
          
                <Card.Body>
                    <Form.Group>
                        <Form.Label className="fw-semibold">Decrypted Filter</Form.Label>
                        <div className="d-flex gap-3 mt-2">
                            <Form.Check
                                inline
                                label="All"
                                name="decryptedFilter"
                                type="radio"
                                id="decrypted-all"
                                value="all"
                                checked={selectedDecryptedFilter === null}
                                onChange={handleDecryptedChange}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="Decrypted"
                                name="decryptedFilter"
                                type="radio"
                                id="decrypted-yes"
                                value="true"
                                checked={selectedDecryptedFilter === true}
                                onChange={handleDecryptedChange}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="Not Decrypted"
                                name="decryptedFilter"
                                type="radio"
                                id="decrypted-no"
                                value="false"
                                checked={selectedDecryptedFilter === false}
                                onChange={handleDecryptedChange}
                                className="cursor-pointer"
                            />
                        </div>
                    </Form.Group>
                </Card.Body>
          
                <Card.Body>
                    <Form.Group>
                        <Form.Label className="fw-semibold">Downloaded Filter</Form.Label>
                        <div className="d-flex gap-3 mt-2">
                            <Form.Check
                                inline
                                label="All"
                                name="downloadedFilter"
                                type="radio"
                                id="downloaded-all"
                                value="all"
                                checked={selectedDownloadedFilter === null}
                                onChange={handleDownloadedChange}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="Downloaded"
                                name="downloadedFilter"
                                type="radio"
                                id="downloaded-yes"
                                value="true"
                                checked={selectedDownloadedFilter === true}
                                onChange={handleDownloadedChange}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="Not Downloaded"
                                name="downloadedFilter"
                                type="radio"
                                id="downloaded-no"
                                value="false"
                                checked={selectedDownloadedFilter === false}
                                onChange={handleDownloadedChange}
                                className="cursor-pointer"
                            />
                        </div>
                    </Form.Group>
                </Card.Body>

                <Card.Body>
                    <Form.Group>
                        <Form.Label className="fw-semibold">Batch Complete Filter</Form.Label>
                        <div className="d-flex gap-3 mt-2">
                            <Form.Check
                                inline
                                label="All"
                                name="batchCompleteFilter"
                                type="radio"
                                id="batchComplete-all"
                                value="all"
                                checked={selectedBatchCompleteFilter === null}
                                onChange={handleBatchCompleteChange}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="BatchComplete"
                                name="batchCompleteFilter"
                                type="radio"
                                id="batchComplete-yes"
                                value="true"
                                checked={selectedBatchCompleteFilter === true}
                                onChange={handleBatchCompleteChange}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="Not Batch Complete"
                                name="batchCompleteFilter"
                                type="radio"
                                id="batchComplete-no"
                                value="false"
                                checked={selectedBatchCompleteFilter === false}
                                onChange={handleBatchCompleteChange}
                                className="cursor-pointer"
                            />
                        </div>
                    </Form.Group>
                </Card.Body>

                <Card.Body>
                    <Form.Group>
                        <Form.Label className="fw-semibold">Processing Filter</Form.Label>
                        <div className="d-flex gap-3 mt-2">
                            <Form.Check
                                inline
                                label="All"
                                name="processingFilter"
                                type="radio"
                                id="processing-all"
                                value="all"
                                checked={selectedProcessingFilter === null}
                                onChange={handleProcessingChange}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="Processing"
                                name="processingFilter"
                                type="radio"
                                id="processing-yes"
                                value="true"
                                checked={selectedProcessingFilter === true}
                                onChange={handleProcessingChange}
                                className="cursor-pointer"
                            />
                            <Form.Check
                                inline
                                label="Not Processing"
                                name="processingFilter"
                                type="radio"
                                id="processing-no"
                                value="false"
                                checked={selectedProcessingFilter === false}
                                onChange={handleProcessingChange}
                                className="cursor-pointer"
                            />
                        </div>
                    </Form.Group>
                </Card.Body>
            </Card>

            <div className="d-flex justify-content-between align-items-center mt-auto">
                <Button
                    variant="outline-secondary"
                    onClick={handleClearFilters}
                    className="fw-semibold"
                    style={{ minWidth: 110 }}
                >
                    <FontAwesomeIcon icon={faTimes} /> Clear
                </Button>
                <Button
                    variant="primary"
                    onClick={handleFilterClick}
                    className="fw-semibold d-flex align-items-center justify-content-center"
                    style={{ minWidth: 110 }}
                >
                    <FontAwesomeIcon icon={faFilter} /> <span className="ms-2">Apply</span>
                </Button>
            </div>
        </div>
    );
};

export default IngestionFilterModal;
