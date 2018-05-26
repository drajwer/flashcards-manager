import {apiUrls} from './apiUrls';
import { getApiCall, postApiCall } from './index';

export function loginApiCall(username, password) {
    return fetch(apiUrls.tokenEndpoint, {
        method: 'POST',
        body: 
            `client_secret=${apiUrls.clientSecret}&client_id=${apiUrls.clientName}&username=${username}&password=${password}&grant_type=password`,
        headers: new Headers({
          'Content-Type': 'application/x-www-form-urlencoded'
        })
})
.then(response => {
    let json = response.json();
    if (response.status != 200) {
      return json.then(err => {throw err;});//throw json;
    }
    return json;
    });
}

export function registerApiCall(username, name, surname, password, confirmPassword, bearerToken) {
  const url = apiUrls.registerAction;
  const data = {
    username,
    name,
    surname,
    password,
    confirmPassword
  };
  return postApiCall(url, data, bearerToken);
}