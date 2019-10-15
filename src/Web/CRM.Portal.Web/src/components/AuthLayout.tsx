import React, { Suspense, useContext, useEffect } from "react";
import { RouteChildrenProps } from "react-router";
import { Container } from "reactstrap";

import { AppCtx } from "contexts";

interface LayoutProps {
    title: string;
    // children: React.ReactNode;
    // history: RouteChildrenProps;
}

const Topbar = React.lazy(() => import("./Topbar"));
const Navbar = React.lazy(() => import("./Navbar"));
const Footer = React.lazy(() => import("./Footer"));
const loading = () => <div className="text-center">Loading...</div>;

const toggleRightSidebar = () => {
    document.body.classList.toggle("right-bar-enabled");
};

// const Layout: React.FC<LayoutProps> = props => {
export default (props: any) => {
    const { state } = useContext(AppCtx);

    useEffect(() => {
        if (!state.notAuthenticated) {
            props.history.push("/authentication/401");
        }
    }, [state.notAuthenticated]);

    return (
        <div className="app">
            <header id="topnav">
                <Suspense fallback={loading()}>
                    <Topbar rightSidebarToggle={toggleRightSidebar}></Topbar>
                    <Navbar />
                </Suspense>
            </header>

            <div className="wrapper">
                <Container fluid>
                    <Suspense fallback={loading()}>{props.children}</Suspense>
                </Container>
            </div>
            <Footer />
        </div>
    );
};

// export default Layout;
