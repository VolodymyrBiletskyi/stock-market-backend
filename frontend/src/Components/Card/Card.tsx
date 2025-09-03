import React from 'react'
import './Card.css'

interface Props  {
  companyName: string;
  ticker: string;
  price: number;
}

const Card = ({companyName, ticker, price} : Props) => {
  return <div className ="Card">
    <img
        src = "https://upload.wikimedia.org/wikipedia/commons/f/fa/Apple_logo_black.svg"
        alt = "Image"
    />
    <div className="details">
        <h2>{companyName} ({ticker})</h2>
        <p>{price}</p>
    </div>
    <p className="info">Lorem, ipsum dolor sit amet consectetur adipisicing elit. Consequuntur, at?</p>
 </div>;
}

 export default Card;