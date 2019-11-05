import React from "react";
import { Row, Col, Card, CardBody } from "reactstrap";

import Loader from "components/Loader";

interface DashboardProps {
  loading: boolean;
}

export default (props: DashboardProps) => {
  React.useEffect(() => {
    setTimeout(() => {}, 100000);
  }, []);

  return (
    <>
      <div className="">
        {props.loading && <Loader />}
        <Row>
          <Col lg={12}>
            <div className="page-title-box">
              <div className="page-title-right">
                <ol className="breadcrumb m-0">
                  <li className="breadcrumb-item">
                    <a href="/">CRM</a>
                  </li>
                  <li className="breadcrumb-item active">Dashboard</li>
                </ol>
              </div>
              <h4 className="page-title">Dashboard</h4>
            </div>
          </Col>
        </Row>

        <Row>
          <Col lg={12}>
            <Card>
              <CardBody>Hello this is dashboard content</CardBody>
            </Card>
          </Col>
        </Row>
      </div>
    </>
  );
};
