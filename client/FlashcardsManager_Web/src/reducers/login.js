const initialState = {
    bearerToken: null,
    isFetching: false
};

import { RECEIVE_ACCESS_TOKEN, LOGOUT, REQUEST_USER, RECEIVED_USER } from '../actions/login';

const login = (state = initialState, action) => {
    switch (action.type) {
        case RECEIVE_ACCESS_TOKEN:
            return {
                ...state,
                bearerToken: action.bearerToken
            };
        case REQUEST_USER:
            return {
                ...state,
                isFetching: true
            };
        case REQUEST_USER:
            return {
                ...state,
                user: action.user,
                isFetching: false
            };
        case LOGOUT:
            return {
                ...initialState
            };
        default:
            return state;
    }
}

export default login;