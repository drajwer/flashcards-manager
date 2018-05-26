// import React from 'react';
// import { render } from 'react-dom';

// import { Provider } from 'react-redux';
// import { createStore, applyMiddleware } from 'redux';
// import thunk from 'redux-thunk';
// import createLogger from 'redux-logger';
// import { AppContainer } from 'react-hot-loader';
// import createOidcMiddleware, { createUserManager, OidcProvider, reducer } from 'redux-oidc';
// import Oidc from 'oidc-client';

// import weatherApp from './reducers';
// import App from './components/App';


// import ons from 'onsenui';
// import 'onsenui/css/onsenui.css';
// import 'onsenui/css/onsen-css-components.css';

// import { addLocationAndFetchWeather } from './actions';

// import { apiUrls } from './api/apiUrls';

// const config = {
//   authority: apiUrls.host,
//   client_id: "wpf",
//   redirect_uri: "https://localhost/oidc",
//   post_logout_redirect_uri: "https://localhost/oidc",
//   silent_redirect_uri: "https://localhost/oidc",

//   response_type: "id_token token",
//   scope: "flashcardsScope",

//   automaticSilentRenew: true,

//   filterProtocolClaims: true,
//   loadUserInfo: true,

//   popupNavigator: new Oidc.CordovaPopupNavigator(),
//   iframeNavigator: new Oidc.CordovaIFrameNavigator()
// }

// // a custom shouldValidate function
// const shouldValidate = (state, action) => {
//   return !action.type === 'DONT_VALIDATE';
// }

// // create a user manager instance
// const userManager = createUserManager(config);

// // create the middleware
// const oidcMiddleware = createOidcMiddleware(userManager, shouldValidate);

// // configure your reducers
// const logger = createLogger();

// const store = createStore(weatherApp,
//   window.devToolsExtension ? window.devToolsExtension() : f => f,
//   process.env.NODE_ENV === 'production'
//     ? applyMiddleware(thunk)
//     : applyMiddleware(thunk, logger)
// );

// const rootElement = document.getElementById('root');

// ons.ready(() => render(
//   <AppContainer>
//     <Provider store={store}>
//       <OidcProvider store={store} userManager={userManager}>
//         <App />
//       </OidcProvider>
//     </Provider>
//   </AppContainer>,
//   rootElement
// ));

// if (module.hot) {
//   module.hot.accept('./components/App', () => {
//     const NextApp = require('./components/App').default;
//     render(
//       <AppContainer>
//         <Provider store={store}>
//           <OidcProvider store={store} userManager={userManager}>
//             <NextApp />
//           </OidcProvider>
//         </Provider>
//       </AppContainer>,
//       rootElement
//     );
//   });
// }
