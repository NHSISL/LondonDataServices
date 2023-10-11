import React, { ReactElement, useEffect } from "react";
import '../styles/base.scss';
import 'nhsuk-frontend/packages/polyfills'; import { UserProfile } from "./UserProfile";
import { NavigationBar } from "./NavigationBar";
import { BaseFooter } from "./BaseFooter";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faB, faBars } from "@fortawesome/free-solid-svg-icons";
import SidebarBase from "./bases/layouts/Sidebar/SidebarBase";
import SidebarBaseNav from "./bases/layouts/Sidebar/SidebarBase.Nav";
import HeaderBase from "./bases/layouts/Header/HeaderBase";
import HeaderBaseNav from "./bases/layouts/Header/HeaderBase.Nav";
import { HeaderBar } from "./HeaderBar";
import ContentBase from "./bases/layouts/Content/ContentBase";
import FooterBase from "./bases/layouts/Footer/FooterBase";

type PageLayoutParameters = {
    children: ReactElement
}

export const PageLayout = ({ children }: PageLayoutParameters) => {
    useEffect(() => {
        const menuToggle = document.getElementById("menu-toggle");
        const wrapper = document.getElementById("wrapper");
        const header = document.querySelector("header");
        const footer = document.querySelector("footer");
        const body = document.body;

        if (menuToggle && wrapper && header && footer) {
            menuToggle.addEventListener("click", function (e) {
                e.preventDefault();
                wrapper.classList.toggle("toggled");
                header.classList.toggle("toggled");
                footer.classList.toggle("toggled");
                body.style.overflowX = "hidden";
                setTimeout(function () { body.style.overflowX = "auto"; }, 1500);
            });
        }

        const footerElement = document.querySelector("footer");
        const pageContentWrapper = document.getElementById("page-content-wrapper");

        if (footerElement && pageContentWrapper) {
            const currentFooterHeight = window.getComputedStyle(footerElement).height;
            const newContentHeight = `calc(100vh - 61px - ${currentFooterHeight})`;
            pageContentWrapper.style.height = newContentHeight;
        }
    }, []);

    return (<>
        <div id="wrapper" className="toggled">
            <SidebarBase>
                <SidebarBaseNav>
                    <NavigationBar />
                </SidebarBaseNav>
            </SidebarBase>

            <HeaderBase>
                <HeaderBaseNav>
                   <HeaderBar></HeaderBar>
                </HeaderBaseNav>
            </HeaderBase>

            <ContentBase>
                {children}
            </ContentBase>
                
            <FooterBase>
                <BaseFooter />
            </FooterBase>
        </div>
    </>)
}

