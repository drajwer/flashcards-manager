const initialState = {
    isFetching: false,
    isInvalid: false,
    showToast: false,
    message: '',
    categories: [],
    categoryId: -1,
};
import {
    RECEIVE_OWN_CATEGORIES, REQUEST_OWN_CATEGORIES, SET_FETCH_ERROR_MANAGE_CATEGORIES, SELECT_OWN_CATEGORY,
    REQUEST_ADD_CATEGORY, REQUEST_UPDATE_CATEGORY, REQUEST_DELETE_CATEGORY,
    RESPONSE_ADD_CATEGORY, RESPONSE_UPDATE_CATEGORY, RESPONSE_DELETE_CATEGORY,
    SET_ADD_CATEGORY_ERROR, SET_UPDATE_CATEGORY_ERROR, SET_DELETE_CATEGORY_ERROR, RESET_TOAST
} from '../actions/manageCategories';
import { RESET_STATE } from '../actions';

const manageCategories = (state = initialState, action) => {
    switch (action.type) {
        case RESET_STATE:
            return {
                ...initialState
            };
        case SELECT_OWN_CATEGORY:
            return {
                ...state,
                categoryId: action.categoryId
            };
        case REQUEST_UPDATE_CATEGORY:
        case REQUEST_DELETE_CATEGORY:
        case REQUEST_ADD_CATEGORY:
        case REQUEST_OWN_CATEGORIES:
            return {
                ...state,
                isFetching: true,
                isInvalid: false,
            };
        case RESPONSE_UPDATE_CATEGORY:
            return responseUpdateCategory(state, action.category, action.isPublished);
        case RESPONSE_DELETE_CATEGORY:
            return responseDeleteCategory(state, action.id);
        case RESPONSE_ADD_CATEGORY:
            return responseAddCategory(state, action.category);
        case RECEIVE_OWN_CATEGORIES:
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
        case SET_ADD_CATEGORY_ERROR:
        case SET_UPDATE_CATEGORY_ERROR:
        case SET_DELETE_CATEGORY_ERROR:
        case SET_FETCH_ERROR_MANAGE_CATEGORIES:
            return {
                ...state,
                isInvalid: true
            };

        default:
            return state;
    }
}

const responseAddCategory = (state, category) => {
    let categories = state.categories;
    categories.push(category);
    return {
        ...state,
        isFetching: false,
        showToast: true,
        message: 'Pomyślnie dodano kategorię',
        categories: categories
    };
};

const responseDeleteCategory = (state, id) => {
    let categories = state.categories;
    let toDel = state.categories.find(c => c.id === id);
    const index = categories.indexOf(toDel);
    categories.splice(index, 1);
    return {
        ...state,
        isFetching: false,
        showToast: true,
        message: 'Pomyślnie usunięto kategorię',
        categories: categories
    };
};

const responseUpdateCategory = (state, category, isPublished = false) => {
    let categories = state.categories;
    let oldCategory = state.categories.find(c => c.id === category.id);
    const index = categories.indexOf(oldCategory);
    categories[index] = category;
    return {
        ...state,
        isFetching: false,
        showToast: true,
        message: isPublished ? 'Kategoria czeka na zaakceptowanie' : 'Pomyślnie edytowano kategorię',
        categories: categories
    };
};


export default manageCategories;