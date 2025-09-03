import React from 'react'
import Card from '../Card/Card'

interface Props{}

export const Cardlist = (props : Props) => {
  return (
    <div>
    <Card companyName='Apple' ticker='AAPL' price={100} />
    <Card companyName='Microsoft' ticker='MSFT' price={200}/>
    <Card companyName='Tesla' ticker='TSLA' price={300}/>
  </div>
  ); 
};
