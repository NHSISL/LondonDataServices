import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { UserProfile } from './UserProfile';
import { faBars } from '@fortawesome/free-solid-svg-icons';

export const HeaderBar = () => {

    return <>
        <a href="#menu-toggle" className="btn btn-light rounded-circle" id="menu-toggle">
            <FontAwesomeIcon icon={faBars} title="menu" className="text-secondary" />
        </a>

        <span className="navbar-brand me-auto ms-4">
            <strong>The <span className="text-hero">London</span> Data Service</strong>
        </span>

        <ul className="nav justify-content-end">
            <li className="nav-item">
               <UserProfile />
            </li>
        </ul>
    </>
}




