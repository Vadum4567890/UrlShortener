import './components/styles/App.css';
import React from 'react';
import { BrowserRouter as Router, Link } from 'react-router-dom';
import AppRoutes from './components/AppRoutes';

const App = () => {
  return (
   <>
    <Router>
      <div>
        <nav>
          <ul class="no-bullets">
            <li><Link to="/">Головна</Link></li>
            <li><Link to="/add">Додати новий URL</Link></li>
          </ul>
        </nav>
          <AppRoutes/>
      </div>
    </Router>
   </> 
  )
}

export default App;
