import React, { Fragment } from 'react';
import { Route } from 'react-router-dom';
import Dashboard from "../../features/dashboard/Dashboard"

const App: React.FC = () => {
  return (
    <Fragment>
      <Route exact path="/" component={Dashboard} />
    </Fragment>
  );
}

export default App;
