import {
    RECEIVE_ADMIN_CATEGORIES, REQUEST_ADMIN_CATEGORIES, SET_FETCH_ERROR_ADMIN_CATEGORIES,
    REQUEST_ACCEPT_CATEGORY, RESPONSE_ACCEPT_CATEGORY, SET_ACCEPT_CATEGORY_ERROR,
    REQUEST_REJECT_CATEGORY, RESPONSE_REJECT_CATEGORY, SET_REJECT_CATEGORY_ERROR,
    RESET_TOAST
} from '../actions/admin';

import { RESET_STATE } from '../actions';

const initialState = {
    isFetching: false,
    isInvalid: false,
    showToast: false,
    message: '',
    categories: [],
    categoryId: -1,
};
import { categoryAvailability } from '../api/types';
const manageCategories = (state = initialState, action) => {
    switch (action.type) {
        case RESET_STATE:
            return {
                ...initialState
            };
        case REQUEST_ACCEPT_CATEGORY:
        case REQUEST_REJECT_CATEGORY:
        case REQUEST_ADMIN_CATEGORIES:
            return {
                ...state,
                isFetching: true,
                isInvalid: false,
            };
        case RESPONSE_REJECT_CATEGORY:
            return responseChangeAvailability(state, action.id, false);
        case RESPONSE_ACCEPT_CATEGORY:
            return responseChangeAvailability(state, action.id, true);
        case RECEIVE_ADMIN_CATEGORIES:
            return {
                ...state,
                categories: action.categories,
                isFetching: false
            };
        case RESET_TOAST:
            return {
                ...state,
                showToast: false,
                message: ''
            };
        case SET_ACCEPT_CATEGORY_ERROR:
        case SET_REJECT_CATEGORY_ERROR:
        case SET_FETCH_ERROR_ADMIN_CATEGORIES:
            return {
                ...state,
                isInvalid: true
            };

        default:
            return state;
    }
}

const responseChangeAvailability = (state, id, isPublished = false) => {
    let categories = state.categories;
    let toDel = state.categories.find(c => c.id === id);
    const index = categories.indexOf(toDel);
    categories.splice(index, 1);

    return {
        ...state,
        isFetching: false,
        showToast: true,
        message: isPublished ? 'Pomyślnie zaakceptowano kategorię' : 'Pomyślnie odrzucono kategorię',
        categories: categories
    };
};


export default manageCategories;