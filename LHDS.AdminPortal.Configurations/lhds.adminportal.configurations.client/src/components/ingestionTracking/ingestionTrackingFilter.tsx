import React, { ChangeEvent, FunctionComponent, useState } from "react";
import { Modal} from "react-bootstrap"
import '../../styles/base.scss';
import ButtonBase from "./../bases/buttons/ButtonBase";
import SelectInputBase from "./../bases/inputs/SelectInputBase";
import { faSave, faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { lookupViewService } from "../../services/views/lookups/lookupViewService";
import { SupplierView } from "../../models/views/components/suppliers/supplierView";
import { Option } from "../../models/options/option";

interface FilterModalProps {
    onClose: () => void;
    onAddFilter: (supplier: SupplierView) => void;
}

const IngestionFilterModal: FunctionComponent<FilterModalProps> = (props) => {
    const {
        onClose,
        onAddFilter
    } = props;

    const { mappedSuppliers: suppliersRetrieved } = lookupViewService.useGetSupplierList("");
    const [supplier, setSupplier] = useState<SupplierView>(new SupplierView(crypto.randomUUID()));

    const handleFilterClick = () => {
        onAddFilter(supplier);
        onClose();
    };

    const handleChange = (
        event: ChangeEvent<HTMLInputElement>
            | ChangeEvent<HTMLTextAreaElement>
            | ChangeEvent<HTMLSelectElement>) => {
        const addSupplier = {
            ...supplier,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setSupplier(addSupplier);
    };

    const supplierOptions: Array<Option> = [
        { id: "", name: "All Suppliers" },
        ...suppliersRetrieved.map((supplier) => {
            return { id: supplier.id.toString() || 0, name: supplier.name || "" };
        }),
    ];

    return (<>
        <div className="filter-modal">
            <Modal.Dialog>
                <Modal.Header>
                    <Modal.Title>Ingestion Filter</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <SelectInputBase
                        id="Id"
                        name="id"
                        value={supplier.id.toString()}
                        required={false}
                        label=""
                        options={supplierOptions}
                        onChange={handleChange} />
                        <br />
            </Modal.Body>
            <Modal.Footer>
                <ButtonBase onClick={handleFilterClick} add>
                    <FontAwesomeIcon icon={faSave} />
                    &nbsp;Save
                </ButtonBase>
                <ButtonBase onClick={onClose} remove>
                    <FontAwesomeIcon icon={faTimes} />
                    &nbsp;Close
                </ButtonBase>
            </Modal.Footer>
        </Modal.Dialog>
    </div >
    </>
  );
};

export default IngestionFilterModal;