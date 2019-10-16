import React, { Suspense, useContext, useEffect } from "react";
import { RouteComponentProps } from "react-router";
import { Container } from "reactstrap";

import { AppCtx } from "contexts";
import Loader from "components/Loader";
import { AuthService } from "services";

interface LayoutProps extends RouteComponentProps {
    title: string;
}

const Topbar = React.lazy(() => import("./Topbar"));
const Navbar = React.lazy(() => import("./Navbar"));
const Footer = React.lazy(() => import("./Footer"));

const toggleRightSidebar = () => {
    document.body.classList.toggle("right-bar-enabled");
};

const Layout: React.FC<LayoutProps> = ({ children, location }) => {
    const { state } = useContext(AppCtx);

    useEffect(() => {
        if (!state.authenticated) {
            AuthService.authenticateUser(location);
        }
    }, [state.authenticated, location]);

    return (
        <>
            {!state.authenticated && <Loader />}
            {state.authenticated && (
                <div className="app">
                    <header id="topnav">
                        <Suspense fallback={<Loader />}>
                            <Topbar rightSidebarToggle={toggleRightSidebar}></Topbar>
                            <Navbar />
                        </Suspense>
                    </header>

                    <div className="wrapper">
                        <Container fluid>
                            <Suspense fallback={<Loader />}>{children}</Suspense>
                        </Container>
                    </div>
                    <Footer />
                </div>
            )}
        </>
    );
};

export default Layout;
