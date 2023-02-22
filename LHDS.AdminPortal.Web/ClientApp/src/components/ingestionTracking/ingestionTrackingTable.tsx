import { debounce } from "lodash";
import React, { useMemo, useState } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";
import SearchBase from "../bases/inputs/SearchBase";
import IngestionTrackingRow from "./ingestionTrackingRow";

const IngestionTrackingTable = () => {

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");

    const handleDebounce = useMemo(
        () => debounce((value: string) => {
            setDebouncedTerm(value)
        }, 500)
        , []);


    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    }

    return (
        <div className="infiniteScollContainer">
            <CardBase >
                <CardBaseBody>
                    <CardBaseTitle>
                        Supplier Data
                    </CardBaseTitle>
                    <CardBaseContent>
                        
                        <SearchBase id="search" label="Search By Filename" value={searchTerm} onChange={(e) => { handleSearchChange(e.currentTarget.value) }} />
                        <TableBase>
                            <TableBaseTbody>
                                <IngestionTrackingRow/>
                            </TableBaseTbody>
                        </TableBase>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div >
    );
}
export default IngestionTrackingTable;