import { useState } from 'react'
import './App.css'
import { Cardlist } from './Components/CardList/Cardlist'

function App() {
  const [count, setCount] = useState(0)

  return (
    <div className="App">
      <Cardlist />
    </div>
  );
}

export default App
