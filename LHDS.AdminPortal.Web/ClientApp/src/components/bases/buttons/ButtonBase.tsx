import React, { FunctionComponent, HTMLProps } from "react";
import {Button } from 'nhsuk-react-components'
import classNames from 'classnames';
import "./ButtonBaseStyle.scss"

interface ButtonBaseProps extends HTMLProps<HTMLButtonElement> {
    id?: string;
    title?: string;
    onClick: (event: any) => void;
    children?: React.ReactNode;
    type?: 'button' | 'submit' | 'reset';
    disabled?: boolean;
    secondary?: boolean;
    reverse?: boolean;
    add?: boolean;
    edit?: boolean;
    remove?: boolean;
    cancel?: boolean;
    view?: boolean;
    as?: 'button';
}

const ButtonBase: FunctionComponent<ButtonBaseProps> = (
    {
        id,
        title,
        children,
        onClick,
        add,
        edit,
        remove,
        cancel,
        view,
        className,
        ...rest
    }
) => {
    return (
        <Button style={{marginRight: "3px"} }
            id={id}
            title={title}
            onClick={onClick}
            className={classNames(
                { 'nhsuk-buttonBlue': add },
                { 'nhsuk-buttonBlue': edit },
                { 'nhsuk-buttonRed': remove },
                { 'nhsuk-buttonGrey': cancel },
                { 'nhsuk-buttonGreen': view },
                className,
            )}
            {...rest}
        >
            {children}
          </Button >
    );
}

ButtonBase.defaultProps = {
    type: 'submit',
}

export default ButtonBase;