import React, { Suspense } from "react";
import { BrowserRouter, Route } from "react-router-dom";
import Loadable from "react-loadable";

// Themes
import "./assets/scss/DefaultTheme.scss";

import { routes } from "./Route";
import withLayout from "hoc/withLayout";
import { AppCtxProvider } from "contexts";

import { ApolloProvider } from "@apollo/react-hooks";
import ApolloClient from "apollo-boost";

// Lazy loading and code splitting -
// Derieved idea from https://blog.logrocket.com/lazy-loading-components-in-react-16-6-6cea535c0b52
const loading = () => <div></div>;

const client = new ApolloClient({
  uri: "/graphql"
});

const AuthLayout = Loadable({
  loader: () => import("components/AuthLayout"),
  render(loaded, props) {
    let Component = loaded.default;
    return <Component {...props} />;
  },
  loading
});

const NonAuthLayout = Loadable({
  loader: () => import("components/NonAuthLayout"),
  render(loaded, props) {
    let Component = loaded.default;
    return <Component {...props} />;
  },
  loading
});

export default () => {
  return (
    <>
      <BrowserRouter>
        <AppCtxProvider>
          <ApolloProvider client={client}>
            {routes.map((route, index) => {
              return (
                <Route
                  key={index}
                  path={route.path}
                  exact={route.exact}
                  component={withLayout(props => {
                    const Layout = route.useAuthLayout ? AuthLayout : NonAuthLayout;
                    return (
                      <Suspense fallback={loading()}>
                        <Layout {...props} title={route.title ? route.title : ""}>
                          <route.component {...props} />
                        </Layout>
                      </Suspense>
                    );
                  })}
                />
              );
            })}
          </ApolloProvider>
        </AppCtxProvider>
      </BrowserRouter>
    </>
  );
};
