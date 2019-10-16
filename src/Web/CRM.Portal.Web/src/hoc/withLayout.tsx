import React, { Component } from "react";
import {  RouteComponentProps } from "react-router";

const withLayout = <P extends object & RouteComponentProps>(WrappedComponent: React.ComponentType<P>) => {
    return class extends Component<P> {
        render() {
            return <WrappedComponent {...this.props} />;
        }
    };
};

export default withLayout;
