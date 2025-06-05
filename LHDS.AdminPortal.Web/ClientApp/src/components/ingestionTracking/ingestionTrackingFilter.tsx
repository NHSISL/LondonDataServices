import React, { ChangeEvent, FunctionComponent, useState } from "react";
import { Modal, Form, Button } from "react-bootstrap";
import '../../styles/base.scss';
import ButtonBase from "./../bases/buttons/ButtonBase";
import { faFilter, faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { lookupViewService } from "../../services/views/lookups/lookupViewService";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import { Guid } from "guid-typescript";

interface FilterModalProps {
    onClose: () => void;
    onAddFilter: (supplier: SupplierView | null, decryptedFilter?: boolean, downloadedFilter?: boolean) => void;
    selectedSupplierId: string;
    initialDecryptedFilter?: boolean | null;
    initialDownloadedFilter?: boolean | null;
}

const IngestionFilterModal: FunctionComponent<FilterModalProps> = ({
    onClose,
    onAddFilter,
    selectedSupplierId: initialSelectedSupplierId,
    initialDecryptedFilter = null,
    initialDownloadedFilter = null
}) => {
    const { mappedSuppliers: suppliersRetrieved } = lookupViewService.useGetTrackedSupplierList("");
    const [selectedSupplierId, setSelectedSupplierId] = useState<string>(initialSelectedSupplierId);
    const [selectedDecryptedFilter, setSelectedDecryptedFilter] = useState<boolean | null>(initialDecryptedFilter);
    const [selectedDownloadedFilter, setSelectedDownloadedFilter] = useState<boolean | null>(initialDownloadedFilter);

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

    const handleClearFilters = () => {
        const clearedSupplierId = "";
        const clearedDecryptedFilter = null;
        const clearedDownloadedFilter = null;

        setSelectedSupplierId(clearedSupplierId);
        setSelectedDecryptedFilter(clearedDecryptedFilter);
        setSelectedDownloadedFilter(clearedDownloadedFilter);

        // Call onAddFilter directly with cleared values
        let supplier: SupplierView | null = null;
        if (clearedSupplierId !== "") {
            const selectedLookup = suppliersRetrieved.find(s => s.id === clearedSupplierId);
            if (selectedLookup) {
                supplier = new SupplierView(Guid.parse(selectedLookup.id));
                supplier.name = selectedLookup.name;
            }
        }

        onAddFilter(
            supplier,
            clearedDecryptedFilter === null ? undefined : clearedDecryptedFilter,
            clearedDownloadedFilter === null ? undefined : clearedDownloadedFilter
        );

        onClose();
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
            selectedDownloadedFilter === null ? undefined : selectedDownloadedFilter
        );

        onClose();
    };

    return (
        <Modal show onHide={onClose}  centered>
            <Modal.Header closeButton className="bg-primary text-white">
                <Modal.Title>Ingestion Filter</Modal.Title>
            </Modal.Header>

            <Modal.Body>
                <Form>
                    <Form.Group className="mb-4">
                        <Form.Label><strong>Supplier Filter</strong></Form.Label>
                        <div>
                            <Form.Check
                                inline
                                label="All"
                                name="supplier"
                                type="radio"
                                id="supplier-all"
                                value=""
                                checked={selectedSupplierId === ""}
                                onChange={handleSupplierChange}
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
                                />
                            ))}
                        </div>
                    </Form.Group>

                    <Form.Group className="mb-4">
                        <Form.Label><strong>Decrypted Filter</strong></Form.Label>
                        <div>
                            <Form.Check
                                inline
                                label="All"
                                name="decryptedFilter"
                                type="radio"
                                id="decrypted-all"
                                value="all"
                                checked={selectedDecryptedFilter === null}
                                onChange={handleDecryptedChange}
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
                            />
                        </div>
                    </Form.Group>

                    <Form.Group>
                        <Form.Label><strong>Downloaded Filter</strong></Form.Label>
                        <div>
                            <Form.Check
                                inline
                                label="All"
                                name="downloadedFilter"
                                type="radio"
                                id="downloaded-all"
                                value="all"
                                checked={selectedDownloadedFilter === null}
                                onChange={handleDownloadedChange}
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
                            />
                        </div>
                    </Form.Group>
                </Form>
            </Modal.Body>

            <Modal.Footer>
                <Button variant="warning" onClick={handleClearFilters} className="me-auto">
                    Clear Filters
                </Button>
                <Button variant="primary" onClick={handleFilterClick}>
                    <FontAwesomeIcon icon={faFilter} />&nbsp;Filter
                </Button>
                <Button variant="secondary" onClick={onClose}>
                    <FontAwesomeIcon icon={faTimes} />&nbsp;Close
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default IngestionFilterModal;
