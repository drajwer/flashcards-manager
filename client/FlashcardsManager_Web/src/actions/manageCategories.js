import { getOwnCategoriesApiCall, addCategoryApiCall, deleteCategoryApiCall, updateCategoryApiCall } from '../api/crudApi';
import { categoryAvailability } from '../api/types';

export const REQUEST_OWN_CATEGORIES = 'REQUEST_OWN_CATEGORIES';
export const RECEIVE_OWN_CATEGORIES = 'RECEIVE_OWN_CATEGORIES';
export const SELECT_OWN_CATEGORY = 'SELECT_OWN_CATEGORY';
export const SET_FETCH_ERROR_MANAGE_CATEGORIES = 'SET_FETCH_ERROR_MANAGE_CATEGORIES';

export const REQUEST_ADD_CATEGORY = 'REQUEST_ADD_CATEGORY';
export const RESPONSE_ADD_CATEGORY = 'RESPONSE_ADD_CATEGORY';
export const SET_ADD_CATEGORY_ERROR = 'SET_ADD_CATEGORY_ERROR';

export const REQUEST_UPDATE_CATEGORY = 'REQUEST_UPDATE_CATEGORY';
export const RESPONSE_UPDATE_CATEGORY = 'RESPONSE_UPDATE_CATEGORY';
export const SET_UPDATE_CATEGORY_ERROR = 'SET_UPDATE_CATEGORY_ERROR';

export const REQUEST_DELETE_CATEGORY = 'REQUEST_DELETE_CATEGORY';
export const RESPONSE_DELETE_CATEGORY = 'RESPONSE_DELETE_CATEGORY';
export const SET_DELETE_CATEGORY_ERROR = 'SET_DELETE_CATEGORY_ERROR';

export const RESET_TOAST = 'RESET_TOAST';

export const getOwnCategories = () => {
    return (dispatch, getState) => {
        dispatch(requestOwnCategories());
        getOwnCategoriesApiCall(getState().login.bearerToken)
            .then((categories) => dispatch(receiveOwnCategories(categories)))
            .catch(() => dispatch(setFetchError()));
    };
};


export const addCategory = (name) => {
    return (dispatch, getState) => {
        dispatch(requestAddCategory());
        const token = getState().login.bearerToken;
        addCategoryApiCall(name, categoryAvailability["Private"],token)
            .then((category) => dispatch(responseAddCategory(category)))
            .catch(() => dispatch(setAddCategoryError()));
    };
};

export const updateNameCategory = (id, name) => {
    return (dispatch, getState) => {
        dispatch(requestUpdateCategory());
        const token = getState().login.bearerToken;
        let categories = getState().manageCategories.categories;
        let category = {...categories.find(c => c.id === id)};
        category.name = name;
        updateCategoryApiCall(category ,token)
            .then(() => dispatch(responseUpdateCategory(category)))
            .catch(() => dispatch(setUpdateCategoryError()));
    };
};

export const publishCategory = (id) => {
    return (dispatch, getState) => {
        dispatch(requestUpdateCategory());
        const token = getState().login.bearerToken;
        let categories = getState().manageCategories.categories;
        let category = {...categories.find(c => c.id === id)};
        category.availability = categoryAvailability['Pending'];

        updateCategoryApiCall(category ,token)
            .then(() => dispatch(responseUpdateCategory(category, true)))
            .catch(() => dispatch(setUpdateCategoryError()));
    };
};

export const deleteCategory = (id) => {
    return (dispatch, getState) => {
        dispatch(requestDeleteCategory());
        const token = getState().login.bearerToken;
        deleteCategoryApiCall(id ,token)
            .then(() => dispatch(responseDeleteCategory(id)))
            .catch(() => dispatch(setDeleteCategoryError()));
    };
};


export const resetToast = () => ({
    type: RESET_TOAST
});

export const selectCategory = id => ({
    type: SELECT_OWN_CATEGORY,
    categoryId: id,
});

const requestOwnCategories = () => ({
    type: REQUEST_OWN_CATEGORIES,
});

const receiveOwnCategories = (categories) => ({
    type: RECEIVE_OWN_CATEGORIES,
    categories
});

const setFetchError = () => ({
    type: SET_FETCH_ERROR_MANAGE_CATEGORIES,
});

const requestAddCategory = () => ({
    type: REQUEST_ADD_CATEGORY,
});

const responseAddCategory = (category) => ({
    type: RESPONSE_ADD_CATEGORY,
    category
});

const setAddCategoryError = () => ({
    type: SET_ADD_CATEGORY_ERROR
});



const requestUpdateCategory = () => ({
    type: REQUEST_UPDATE_CATEGORY,
});

const responseUpdateCategory = (category, isPublished = false) => ({
    type: RESPONSE_UPDATE_CATEGORY,
    category,
    isPublished
});

const setUpdateCategoryError = () => ({
    type: SET_UPDATE_CATEGORY_ERROR
});



const requestDeleteCategory = () => ({
    type: REQUEST_DELETE_CATEGORY,
});

const responseDeleteCategory = (id) => ({
    type: RESPONSE_DELETE_CATEGORY,
    id
});

const setDeleteCategoryError = () => ({
    type: SET_DELETE_CATEGORY_ERROR
});

