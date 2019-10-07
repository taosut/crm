
import React from "react";
import { Table } from 'reactstrap';
import { useQuery } from '@apollo/react-hooks';
import { gql } from "apollo-boost";
import { BeatLoader } from 'react-spinners';
import { Alert } from 'reactstrap';
import { css } from '@emotion/core';

const GET_LEADS = gql`
{
contacts {
    id,
    address {
      city,
      country
    }
  }
}
`;

const override = css`
    width: 60px;
    margin: auto;
`;

export default () => {
  const { loading, error, data } = useQuery(GET_LEADS);

  if (loading) return (
    <div className='sweet-loading'>
      <BeatLoader
      css={override}
        color={'#123abc'}
        loading={loading}
      />
    </div>
  );

  if (error) {
    console.log(error);
    return (
      <Alert color="danger">
        There are something wrong...
    </Alert>
    )
  }

  return (
    <Table>
      <thead>
        <tr>
          <th>#</th>
          <th>Id</th>
          <th>City</th>
          <th>country</th>
        </tr>
      </thead>
      <tbody>
        {data.contacts.map((contact: any, index: any) => (
          <tr key={index}>
            <th scope="row">{index}</th>
            <td>{contact.id}</td>
            <td>{contact.address.city}</td>
            <td>{contact.address.country}</td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
};
