import React, { FunctionComponent, FormEventHandler } from "react";
import { Input, Label, SearchIcon } from 'nhsuk-react-components'

interface SearchBaseProps {
    id: string;
    label?: string;
    onChange: FormEventHandler<HTMLInputElement>;
    placeholder?: string;
    value?: string | number;
    description?: string;
}

const SearchBase: FunctionComponent<SearchBaseProps> = (props) => {
    return (
        <>
            <Label size="s"><SearchIcon />{props.label}</Label>
            <Input
                id="input-example"
                placeholder={props.placeholder}
                value={props.value}
                type="search"
                onChange={props.onChange}>
            </Input>

            {props.description && (<><br /><small>{props.description}</small></>)}
        </>
    );
}

export default SearchBase;