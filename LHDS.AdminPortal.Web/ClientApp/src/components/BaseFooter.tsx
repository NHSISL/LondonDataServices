import React from 'react';
import { Footer } from 'nhsuk-react-components'

import 'nhsuk-frontend/dist/nhsuk.min'
import 'nhsuk-frontend/packages/polyfills';

export const BaseFooter = () => {
    return (
        <>
            <Footer>
                <Footer.List>
                </Footer.List>
                <Footer.Copyright>&copy; North East London</Footer.Copyright>
            </Footer>
        </>
    );
};
