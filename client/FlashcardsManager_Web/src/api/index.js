
export const getApiCall = (url, bearerToken) => {
  return fetch(url, {
    method: 'GET',
    headers: {
      'Authorization':
        'Bearer ' + bearerToken
    },
    mode: 'cors'
  })
    .then(response => {
      if (response.status >= 400) {
        throw new Error(response);
      }

      return response.json();
    }).catch(error => {
      console.log(error);
    });
};

export const postApiCall = (url, data, bearerToken) => {
  return fetch(url, {
    method: 'POST',
    body: JSON.stringify(data),
    headers: new Headers({
      'Content-Type': 'application/json',
      'Authorization':
        'Bearer ' + bearerToken
    })
  }).then(response => {
    if (response.status != 200 && response.status != 201) {
      throw response;
    }
  });
}

export const postWithReturnApiCall = (url, data, bearerToken) => {
  return fetch(url, {
    method: 'POST',
    body: JSON.stringify(data),
    headers: new Headers({
      'Content-Type': 'application/json',
      'Authorization':
        'Bearer ' + bearerToken
    })
  }).then(response => {
    if (response.status != 200 && response.status != 201) {
      throw new Error(response);
    }
    return response.json();
  });
}

export const putApiCall = (url, data, bearerToken) => {
  return fetch(url, {
    method: 'PUT',
    body: JSON.stringify(data),
    headers: new Headers({
      'Content-Type': 'application/json',
      'Authorization':
        'Bearer ' + bearerToken
    })
  }).then(response => {
    if (response.status != 200 && response.status != 201) {
      throw new Error(response);
    }
  });
}

export const deleteApiCall = (url, bearerToken) => {
  return fetch(url, {
    method: 'DELETE',
    headers: new Headers({
      'Content-Type': 'application/json',
      'Authorization':
        'Bearer ' + bearerToken
    })
  }).then(response => {
    if (response.status != 200 && response.status != 201) {
      throw new Error(response);
    }
  });
}