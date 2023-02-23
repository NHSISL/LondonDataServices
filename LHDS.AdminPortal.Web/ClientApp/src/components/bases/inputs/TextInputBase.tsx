import React, { FunctionComponent, ChangeEvent } from "react";
import { Label, Input } from 'nhsuk-react-components'
import { InputGroup, Form } from "react-bootstrap"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAsterisk } from "@fortawesome/free-solid-svg-icons";

interface TextInputBaseProps {
    id: string;
    name: string;
    label?: string;
    placeholder?: string;
    prependLabel?: string;
    appendLabel?: string;
    description?: string;
    required?: boolean;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
    value?: string | number;
    error?: string;
    type?: string;
}

const TextInputBase: FunctionComponent<TextInputBaseProps> = (props) => {
    return (
        <Form.Group>
            {props.label && (<b><Label htmlFor={props.id}>{props.label}</Label> </b>)}
            <div>
                <InputGroup>
                    {
                        props.prependLabel !== undefined
                        && props.prependLabel.length > 0
                        && (
                            <InputGroup.Text>{props.prependLabel}</InputGroup.Text>
                        )}
                    <Input
                        id={props.id}
                        name={props.name}
                        value={props.value}
                        onChange={props.onChange}
                        type={props.type}
                        placeholder={props.placeholder || ""}
                        error={props.error}
                    />
                    {
                        props.appendLabel !== undefined
                        && props.appendLabel.length > 0
                        && (
                            <InputGroup.Text>{props.appendLabel}</InputGroup.Text>
                        )}
                    {
                        props.required &&
                        <span style={{ marginTop: "8px" }}> &nbsp;
                            <FontAwesomeIcon icon={faAsterisk} className="text-danger" title="required" />
                        </span>
                    }
                </InputGroup>
                {props.description && (<><br /><small>{props.description}</small></>)}
            </div>


        </Form.Group>
    );
}

TextInputBase.defaultProps = {
    error: "",
}

export default TextInputBase;