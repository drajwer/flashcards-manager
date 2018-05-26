import React from 'react';
import { render } from 'react-dom';

import { Provider } from 'react-redux';
import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import createLogger from 'redux-logger';
import { AppContainer } from 'react-hot-loader';

import combinedReducers from './reducers';
import App from './components/App';

import ons from 'onsenui';
import 'onsenui/css/onsenui.css';
import 'onsenui/css/onsen-css-components.css';
import moment from 'moment';
import { addLocationAndFetchWeather } from './actions';

import { apiUrls } from './api/apiUrls';

// // push notfications
// if (cordova != null && cordova.plugins != null) {
//   cordova.plugins.notification.local.schedule({
//     id: 1,
//     title: 'Czas na naukę!',
//     text: 'Zajrzyj do aplikacji i zacznij naukę.',
//     every: 'minute',
//     firstAt: moment().add({minute: 2})
// });
// }


// configure your reducers
const logger = createLogger();

const store = createStore(combinedReducers,
  window.devToolsExtension ? window.devToolsExtension() : f => f,
  process.env.NODE_ENV === 'production'
    ? applyMiddleware(thunk)
    : applyMiddleware(thunk, logger)
);

const rootElement = document.getElementById('root');

ons.ready(() => render(
  <AppContainer>
    <Provider store={store}>
        <App />
    </Provider>
  </AppContainer>,
  rootElement
));

if (module.hot) {
  module.hot.accept('./components/App', () => {
    const NextApp = require('./components/App').default;
    render(
      <AppContainer>
        <Provider store={store}>
            <NextApp />
        </Provider>
      </AppContainer>,
      rootElement
    );
  });
}
