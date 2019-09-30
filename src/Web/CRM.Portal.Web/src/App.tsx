import React from "react";
import { BrowserRouter } from "react-router-dom";

import "./App.css";

import { DefaultLayout } from "./containers";
import { AppCtxProvider } from "./contexts";
import { ApolloProvider } from '@apollo/react-hooks';
import ApolloClient from 'apollo-boost';
import { AuthService, LoggerService } from "services";

const client = new ApolloClient({
  uri: 'http://localhost:58134/graphql',
  request: async (operation) => {
    const user = await AuthService.UserManager.getUser();
    const token = user.access_token;

    operation.setContext({
      headers: {
        authorization: token ? `Bearer ${token}` : ''
      }
    });
  }
});


export default () => {
  return (
    <>
      <ApolloProvider client={client}>
        <BrowserRouter>
          <AppCtxProvider>
            <DefaultLayout />
          </AppCtxProvider>
        </BrowserRouter>
      </ApolloProvider>
    </>
  );
};
