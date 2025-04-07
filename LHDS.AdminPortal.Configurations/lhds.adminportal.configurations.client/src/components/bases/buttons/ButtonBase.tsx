/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { FunctionComponent, HTMLProps } from "react";
import classNames from 'classnames';
import "./ButtonBaseStyle.scss"
import { Button } from "react-bootstrap";

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
    info?: boolean
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
        info,
        className
    }
) => 
    {
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
                    { 'nhsuk-buttonYellow': info },
                    className,
                )}
            >
                {children}
            </Button >
        );
    }

ButtonBase.defaultProps = {
    type: 'submit',
}

export default ButtonBase;