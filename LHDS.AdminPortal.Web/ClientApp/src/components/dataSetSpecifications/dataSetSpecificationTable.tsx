import React, { FunctionComponent } from "react";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import { Col, Row } from "react-bootstrap";
import { SecuredComponents } from "../Links";
import securityPoints from "../../SecurityMatrix";
import { Link } from "react-router-dom";
import ButtonBase from "../bases/buttons/ButtonBase";

type DataSetSpecificationTableProps = {};

const DataSetSpecificationTable: FunctionComponent<DataSetSpecificationTableProps> = (props) => {
    
    return (<div>
        <CardBase>
            <CardBaseBody>
                <CardBaseTitle>Data Set Specifications</CardBaseTitle>
                <CardBaseContent>
                    <Row>
                        <Col style={{ textAlign: "right" }}>
                            <SecuredComponents allowedRoles={securityPoints.dataSets.add}>
                                <>
                                    <Link to={''}>
                                        <ButtonBase onClick={() => { }} add>&nbsp;Add DataSet Specification</ButtonBase>
                                    </Link>
                                </>
                            </SecuredComponents>
                        </Col>

                    </Row>

                <p>-- DataSet has no Specifications --</p>
                </CardBaseContent>
            </CardBaseBody>
        </CardBase>

    </div>
        
    );
};

export default DataSetSpecificationTable;