import {getAdminCategoriesApiCall, acceptCategoryApiCall, rejectCategoryApiCall} from '../api/adminApi';

export const REQUEST_ADMIN_CATEGORIES = 'REQUEST_ADMIN_CATEGORIES';
export const RECEIVE_ADMIN_CATEGORIES = 'RECEIVE_ADMIN_CATEGORIES';
export const SET_FETCH_ERROR_ADMIN_CATEGORIES = 'SET_FETCH_ERROR_ADMIN_CATEGORIES';

export const REQUEST_ACCEPT_CATEGORY = 'REQUEST_ACCEPT_CATEGORY';
export const RESPONSE_ACCEPT_CATEGORY = 'RESPONSE_ACCEPT_CATEGORY';
export const SET_ACCEPT_CATEGORY_ERROR = 'SET_ACCEPT_CATEGORY_ERROR';

export const REQUEST_REJECT_CATEGORY = 'REQUEST_REJECT_CATEGORY';
export const RESPONSE_REJECT_CATEGORY = 'RESPONSE_REJECT_CATEGORY';
export const SET_REJECT_CATEGORY_ERROR = 'SET_REJECT_CATEGORY_ERROR';


export const getAdminCategories = () => {
    return (dispatch, getState) => {
        dispatch(requestAdminCategories());
        getAdminCategoriesApiCall(getState().login.bearerToken)
            .then((categories) => dispatch(receiveAdminCategories(categories)))
            .catch(() => dispatch(setFetchError()));
    };
};

export const acceptCategory = (id) => {
    return (dispatch, getState) => {
        dispatch(requestAcceptCategory());
        const token = getState().login.bearerToken;
        let categories = getState().admin.categories;
        let category = {...categories.find(c => c.id === id)};

        acceptCategoryApiCall(category ,token)
            .then(() => dispatch(responseAcceptCategory(id)))
            .catch(() => dispatch(setRejectCategoryError()));
    };
};

export const rejectCategory = (id) => {
    return (dispatch, getState) => {
        dispatch(requestRejectCategory());
        const token = getState().login.bearerToken;
        let categories = getState().admin.categories;
        let category = {...categories.find(c => c.id === id)};

        rejectCategoryApiCall(category ,token)
            .then(() => dispatch(responseRejectCategory(id)))
            .catch(() => dispatch(setRejectCategoryError()));
    };
};


const requestAdminCategories = () => ({
    type: REQUEST_ADMIN_CATEGORIES,
});

const receiveAdminCategories = (categories) => ({
    type: RECEIVE_ADMIN_CATEGORIES,
    categories
});

const setFetchError = () => ({
    type: SET_FETCH_ERROR_ADMIN_CATEGORIES,
});

const requestAcceptCategory = () => ({
    type: REQUEST_ACCEPT_CATEGORY,
});

const responseAcceptCategory = (id) => ({
    type: RESPONSE_ACCEPT_CATEGORY,
    id
});

const setAcceptCategoryError = () => ({
    type: SET_ACCEPT_CATEGORY_ERROR
});


const requestRejectCategory = () => ({
    type: REQUEST_REJECT_CATEGORY,
});

const responseRejectCategory = (id) => ({
    type: RESPONSE_REJECT_CATEGORY,
    id
});

const setRejectCategoryError = () => ({
    type: SET_REJECT_CATEGORY_ERROR
});

