import React from "react";
import { Redirect } from "react-router-dom";
// import WithOidcSecure from "./hoc/WithOidcSecure";

const Dashboard = React.lazy(() => import("pages/Dashboard/Dashboard"));
const OidcCallback = React.lazy(() => import("components/oidc/callback/Callback"));
const OidcSilentCallback = React.lazy(() => import("components/oidc/callback/SilentCallback"));
const NotAuthenticated = React.lazy(() =>
    import("components/oidc/notAuthenticated/NotAuthentication")
);
const Landing = React.lazy(() => import("pages/Landing/Landing"));

const routes: Array<{
    path: string;
    name?: string;
    component: any;
    title?: string;
    exact?: boolean;
    useAuthLayout?: boolean;
}> = [
    {
        path: "/authentication/callback",
        component: OidcCallback,
        title: "Dashboard"
    },
    {
        path: "/authentication/silent_callback",
        component: OidcSilentCallback,
        title: "Dashboard"
    },
    {
        path: "/authentication/401",
        component: NotAuthenticated,
        title: "Dashboard"
    },
    {
        path: "/dashboard",
        name: "dashboard",
        component: Dashboard,
        title: "Dashboard",
        useAuthLayout: true
    },
    {
        path: "/company",
        name: "Company",
        component: Landing,
        title: "Landing",
        useAuthLayout: true
    },
    {
        path: "/",
        exact: true,
        component: () => <Redirect to="/dashboard" />
    }
];

export { routes };
