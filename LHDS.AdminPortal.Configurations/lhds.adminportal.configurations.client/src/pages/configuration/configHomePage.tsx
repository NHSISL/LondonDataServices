import React from 'react';
import { Link } from 'react-router-dom';
import CardBase from '../../components/bases/components/Card/CardBase';
import CardBaseContent from '../../components/bases/components/Card/CardBase.Content';
import CardBaseBody from '../../components/bases/components/Card/CardBase.Body';
import CardBaseTitle from '../../components/bases/components/Card/CardBase.Title';

export const ConfigHomePage = () => {
    return (
        <div>
            <div className="container-fluid py-5 bg-primary text-white">
                <h1 className="display-5 fw-bold">Configuration</h1>
                <p className="col-md-8 fs-4">Area to manage configuration items.</p>
            </div>

            <section>
                <div className="container-fluid">

                    <CardBase>
                        <CardBaseBody>
                            <CardBaseTitle>
                                <Link to={'/configuration/suppliers'} className="linkVisited">
                                    Suppliers
                                </Link>
                            </CardBaseTitle>
                            <CardBaseContent>
                                View, add, edit and remove Suppliers.
                            </CardBaseContent>
                        </CardBaseBody>
                    </CardBase>

                    <CardBase>
                        <CardBaseBody>
                            <CardBaseTitle>
                                <Link to={'/configuration/dataTypes'} className="linkVisited">
                                    Data Types
                                </Link>
                            </CardBaseTitle>
                            <CardBaseContent>
                                View, add, edit and remove Data Types.
                            </CardBaseContent>
                        </CardBaseBody>
                    </CardBase>

                    <CardBase>
                        <CardBaseBody>
                            <CardBaseTitle>
                                <Link to={'/configuration/dataSets'} className="linkVisited">
                                    Data Sets
                                </Link>
                            </CardBaseTitle>
                            <CardBaseContent>
                                View, add, edit and remove Data Sets.
                            </CardBaseContent>
                        </CardBaseBody>
                    </CardBase>
                </div>
            </section>
        </div>
    );
};
