export const RECEIVE_ACCESS_TOKEN = 'RECEIVE_ACCESS_TOKEN';
export const LOGOUT = 'LOGOUT';

export const REQUEST_USER = 'REQUEST_USER';
export const RECEIVED_USER = 'RECEIVED_USER';

export const receiveAccessToken = accessToken => ({
  type:  RECEIVE_ACCESS_TOKEN,
  bearerToken: accessToken
});

export const logout = () => ({
  type: LOGOUT
});

export const requestUser = () => ({
  type: REQUEST_USER
});

export const receivedUser = (user) => ({
  type: RECEIVED_USER,
  user
});