import React, { FunctionComponent } from "react";
import SubscriberAgreementDetailCard from "./subscriberAgreementDetailCard";
import { Button, Form, FormGroup } from "react-bootstrap";
import { Input, Label } from "nhsuk-react-components";
import ButtonBase from "../bases/buttons/ButtonBase";

interface SubscriberAgreementAddProps {
    children?: React.ReactNode;
}

const SubscriberAgreementAdd: FunctionComponent<SubscriberAgreementAddProps> = (props) => {
    const {
        children
    } = props;

    return (
        <Form>
            <FormGroup>
                <Label>Subscriber Agreement Short Name</Label>
                <Input type="text" />
            </FormGroup>
            <ButtonBase onClick={() => {}} add>Add</ButtonBase>
        </Form>
    );
}

export default SubscriberAgreementAdd;