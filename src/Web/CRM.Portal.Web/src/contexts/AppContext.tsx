import React, { useReducer, Reducer, useEffect, Dispatch } from "react";

import { IContextProps } from "./ContextProps";
import { createAction, createActionPayload, ActionsUnion } from "./Actions";
import { LoggerService, AuthService } from "services";
import { User } from "oidc-client";

export const TOGGLE_COLLAPSE = "TOGGLE_COLLAPSE";

export const LOAD_LOGINUSER = "LOAD_LOGINUSER";
export const UNLOAD_LOGINUSER = "UNLOAD_LOGINUSER";

export const LOAD_CONTACTS = "LOAD_CONTACTS";
export const LOAD_CONTACTS_SUCCEED = "LOAD_CONTACTS_SUCCEED";
export const LOAD_CONTACTS_FAILED = "LOAD_CONTACTS_FAILED";

interface LoginUser {
  userName: string;
  email: string;
}

interface Contact {
  id: string;
  firstName: string;
  lastName: string;
}

interface UIState {
  notification: any;
  userLogin: LoginUser | null;
  authenticated: boolean;
  contacts: Contact[];
}

const initialState: UIState = {
  notification: {
    numberOfMsg: 10
  },
  userLogin: null,
  authenticated: false,
  contacts: []
};

const AppCtx = React.createContext({} as IContextProps<UIState>);

const AppActions = {
  loadUserLogin: createActionPayload<typeof LOAD_LOGINUSER, LoginUser>(LOAD_LOGINUSER),
  unloadUserLogin: createAction<typeof UNLOAD_LOGINUSER>(UNLOAD_LOGINUSER),
  loadContacts: createActionPayload<typeof LOAD_CONTACTS, Contact[]>(LOAD_CONTACTS)
};

const reducer = (state: UIState, action: ActionsUnion<typeof AppActions>) => {
  switch (action.type) {
    case LOAD_LOGINUSER:
      return {
        ...state,
        userLogin: action.payload,
        authenticated: true
      };

    case UNLOAD_LOGINUSER:
      return {
        ...state,
        userLogin: null,
        authenticated: false
      };

    case LOAD_CONTACTS:
      return {
        ...state,
        contacts: action.payload
      };

    default:
      return initialState;
  }
};

const onUserLoaded = (dispatch: React.Dispatch<any>) => (user: User) => {
  LoggerService.info("User Loaded");
  dispatch(
    AppActions.loadUserLogin({
      userName: user.profile.preferred_username,
      email: user.profile.email
    })
  );
};

// const onUserUnloaded = (dispatch: React.Dispatch<any>) => () => {
//     debugger;
// //   LoggerService.info("User unloaded");
//   dispatch(AppActions.unloadUserLogin());
// };

const onAccessTokenExpired = (dispatch: React.Dispatch<any>) => async () => {
  LoggerService.info("Token expired.");
  dispatch(AppActions.unloadUserLogin());

  await AuthService.UserManager.signinSilent();
};

const addOidcEvents = (dispatch: Dispatch<any>) => {
  const oidcEvents = AuthService.UserManager.events;

  oidcEvents.addUserLoaded(onUserLoaded(dispatch));
  //   oidcEvents.addUserUnloaded(onUserUnloaded(dispatch));
  oidcEvents.addAccessTokenExpired(onAccessTokenExpired(dispatch));
};

const removeOidcEvents = (dispatch: Dispatch<any>) => {
  const oidcEvents = AuthService.UserManager.events;

  oidcEvents.removeUserLoaded(onUserLoaded(dispatch));
  //   oidcEvents.removeUserUnloaded(onUserUnloaded(dispatch));
  oidcEvents.removeAccessTokenExpired(onAccessTokenExpired(dispatch));
};

const AppCtxProvider = (props: any) => {
  let [state, dispatch] = useReducer<Reducer<UIState, any>>(reducer, initialState);
  let value = { state, dispatch };

  useEffect(() => {
    addOidcEvents(dispatch);

    AuthService.UserManager.getUser().then(oidcUser => {
      if (oidcUser && !oidcUser!.expired) {
        dispatch(
          AppActions.loadUserLogin({
            userName: oidcUser.profile.preferred_username,
            email: oidcUser.profile.email
          })
        );
      }
    });

    return () => removeOidcEvents(dispatch);
  }, []);

  return <AppCtx.Provider value={value}>{props.children}</AppCtx.Provider>;
};

export { AppCtx, AppCtxProvider, AppActions };
