import { Table } from "nhsuk-react-components";
import React from "react";
import { Link } from "react-router-dom";
import securityPoints from "../../SecurityMatrix";
import ButtonBase from "../bases/buttons/ButtonBase";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import TableBase from "../bases/components/Table/TableBase";
import TableBaseTbody from "../bases/components/Table/TableBase.Tbody";
import { SecuredComponents } from "../Links";

const SupplierTable = () => {

    return (
        <div className="infiniteScollContainer">
            <CardBase >
                <CardBaseBody>
                    <CardBaseTitle>
                        Suppliers
                    </CardBaseTitle>
                    <CardBaseContent>
                        <TableBase>
                            <TableBaseTbody>
                                <Table.Row>
                                    <Table.Cell className="middle">EMIS</Table.Cell>
                                    <Table.Cell className="middle">View SFTP files downloaded, with audit and actions</Table.Cell>
                                    <Table.Cell className="middle">
                                        <Link to={'/supplier/'}>
                                            <ButtonBase onClick={() => { }} add>
                                                View
                                            </ButtonBase>
                                        </Link>
                                    </Table.Cell>
                                </Table.Row>
                                <Table.Row>
                                    <Table.Cell className="middle">TPP</Table.Cell>
                                    <Table.Cell className="middle">View Api files downloaded, with audit and actions</Table.Cell>
                                    <Table.Cell className="middle">
                                        <Link to={'/supplier/'}>
                                            <ButtonBase onClick={() => { }} add>
                                                View
                                            </ButtonBase>
                                        </Link>
                                    </Table.Cell>
                                </Table.Row>
                            </TableBaseTbody>
                        </TableBase>
                    </CardBaseContent>
                </CardBaseBody>
            </CardBase>
        </div >
    );
}
export default SupplierTable;