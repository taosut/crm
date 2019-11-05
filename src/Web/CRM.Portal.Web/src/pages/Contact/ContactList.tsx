import React from "react";
import { Row, Col, Card, CardHeader, CardBody, Table, Button } from "reactstrap";
import styled from "styled-components";

import { useQuery } from "@apollo/react-hooks";
import { gql } from "apollo-boost";

import Loader from "components/Loader";

const StyledColActions = styled.td`

`;

interface ContactListProps {
  loading: boolean;
}

const GET_CONTACTS = gql`
  query getContacts {
    contacts {
      id
      firstName
      lastName
      title
      company
    }
  }
`;

export default (props: ContactListProps) => {
  const { loading, data } = useQuery(GET_CONTACTS, {});

  return (
    <>
      {loading && <Loader />}
      {!loading && (
        <>
          <Row>
            <Col lg={12}>
              <div className="page-title-box">
                <div className="page-title-right">
                  <ol className="breadcrumb m-0">
                    <li className="breadcrumb-item">
                      <a href="/">CRM</a>
                    </li>
                    <li className="breadcrumb-item active">Contacts</li>
                  </ol>
                </div>
                <h4 className="page-title">Contacts</h4>
              </div>
            </Col>
          </Row>

          <Row>
            <Col lg={12}>
              <Card>
                <CardHeader>
                  <h3 className="float-left">Contact List</h3>
                  <Button color="primary" className="float-right">
                    <i className="mdi mdi-plus"></i> Add
                  </Button>
                </CardHeader>
                <CardBody>
                  <Table>
                    <thead>
                      <tr>
                        <th>#</th>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Title</th>
                        <th>Company</th>
                        <th>&nbsp;</th>
                      </tr>
                    </thead>
                    <tbody>
                      {data.contacts.map((contact: any, index: any) => (
                        <tr key={contact.id}>
                          <th scope="row">{index + 1}</th>
                          <td>{contact.firstName}</td>
                          <td>{contact.lastName}</td>
                          <td>{contact.title}</td>
                          <td>{contact.company}</td>
                          <StyledColActions>
                            <Button color="secondary">
                              <i className="mdi mdi-details"></i>
                            </Button>
                            &nbsp;
                            <Button color="warning">
                              <i className="mdi mdi-pencil"></i>
                            </Button>
                            &nbsp;
                            <Button color="danger">
                              <i className="mdi mdi-delete"></i>
                            </Button>
                          </StyledColActions>
                        </tr>
                      ))}
                    </tbody>
                  </Table>
                </CardBody>
              </Card>
            </Col>
          </Row>
        </>
      )}
    </>
  );
};
