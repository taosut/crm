import React, { Suspense } from "react";

// loading
const loading = () => <div className="text-center">Loading...</div>;

interface NonAuthLayoutProps {}

const NonAuthLayout: React.FC<NonAuthLayoutProps> = props => {
    return <Suspense fallback={loading()}>{props.children}</Suspense>;
};

export default NonAuthLayout;
