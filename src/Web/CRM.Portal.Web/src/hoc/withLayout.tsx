import React, { Component } from "react";

const withLayout = <P extends object>(WrappedComponent: React.ComponentType<P>) => {
    return class extends Component<P> {
        render() {
            return <WrappedComponent {...this.props} />;
        }
    };
};

export default withLayout;
