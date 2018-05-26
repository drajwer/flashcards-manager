import React from 'react';
import WelcomePage from './WelcomePage';
import { Navigator } from 'react-onsenui';
import { routeComponents } from '../routing/routeComponents';
import { routeLabels } from '../routing/routeLabels';
import LoginPage from '../containers/LoginPage';
import { connect } from 'react-redux';

const renderPage = (obj, navigator) => {
  const key = obj.key;
  const Page = routeComponents[key].component;
  return (<Page key={key} pageKey={key} navigator={navigator} title={routeLabels[key].title} />);
};

export function App(props) {
  if (!props.bearerToken) {
    return (
      <Navigator
      renderPage={renderPage}
      initialRoute={{ key: 'LOGIN_PAGE' }}
    />
    );
  }
  return (
    <Navigator
      renderPage={renderPage}
      initialRoute={{ key: 'MAIN_PAGE' }}
    />
  );
}

function mapStateToProps(state) {
  return {
    bearerToken: state.login.bearerToken,
  };
}

export default connect(mapStateToProps)(App);