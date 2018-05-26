import { getOwnFlashcardsApiCall, addFlashcardApiCall, deleteFlashcardApiCall } from '../api/crudApi';
import { categoryAvailability } from '../api/types';

export const REQUEST_OWN_FLASHCARDS = 'REQUEST_OWN_FLASHCARDS';
export const RECEIVE_OWN_FLASHCARDS = 'RECEIVE_OWN_FLASHCARDS';
export const SELECT_OWN_FLASHCARD = 'SELECT_OWN_FLASHCARD';
export const SET_FETCH_ERROR_MANAGE_FLASHCARDS = 'SET_FETCH_ERROR_MANAGE_FLASHCARDS';

export const REQUEST_ADD_FLASHCARD = 'REQUEST_ADD_FLASHCARD';
export const RESPONSE_ADD_FLASHCARD = 'RESPONSE_ADD_FLASHCARD';
export const SET_ADD_FLASHCARD_ERROR = 'SET_ADD_FLASHCARD_ERROR';

export const REQUEST_DELETE_FLASHCARD = 'REQUEST_DELETE_FLASHCARD';
export const RESPONSE_DELETE_FLASHCARD = 'RESPONSE_DELETE_FLASHCARD';
export const SET_DELETE_FLASHCARD_ERROR = 'SET_DELETE_FLASHCARD_ERROR';

export const RESET_TOAST = 'RESET_TOAST';

export const getOwnFlashcards = (id) => {
    return (dispatch, getState) => {
        dispatch(requestOwnFlashcards());
        getOwnFlashcardsApiCall(id, getState().login.bearerToken)
            .then((flashcards) => dispatch(receiveOwnFlashcards(flashcards)))
            .catch(() => dispatch(setFetchError()));
    };
};


export const addFlashcard = (key, value, keyDesc, valueDesc) => {
    return (dispatch, getState) => {
        dispatch(requestAddFlashcard());
        const token = getState().login.bearerToken;
        const categoryId = getState().manageCategories.categoryId;
        addFlashcardApiCall(key, value, keyDesc, valueDesc, categoryId, token)
            .then((flashcard) => dispatch(responseAddFlashcard(flashcard)))
            .catch(() => dispatch(setAddFlashcardError()));
    };
};

export const deleteFlashcard = (id) => {
    return (dispatch, getState) => {
        dispatch(requestDeleteFlashcard());
        const token = getState().login.bearerToken;
        deleteFlashcardApiCall(id, token)
            .then(() => dispatch(responseDeleteFlashcard(id)))
            .catch(() => dispatch(setDeleteFlashcardError()));
    };
};


export const resetToast = () => ({
    type: RESET_TOAST
});

export const selectFlashcard = id => ({
    type: SELECT_OWN_FLASHCARD,
    flashcardId: id,
});

const requestOwnFlashcards = () => ({
    type: REQUEST_OWN_FLASHCARDS,
});

const receiveOwnFlashcards = (flashcards) => ({
    type: RECEIVE_OWN_FLASHCARDS,
    flashcards
});

const setFetchError = () => ({
    type: SET_FETCH_ERROR_MANAGE_FLASHCARDS,
});

const requestAddFlashcard = () => ({
    type: REQUEST_ADD_FLASHCARD,
});

const responseAddFlashcard = (flashcard) => ({
    type: RESPONSE_ADD_FLASHCARD,
    flashcard
});

const setAddFlashcardError = () => ({
    type: SET_ADD_FLASHCARD_ERROR
});




const requestDeleteFlashcard = () => ({
    type: REQUEST_DELETE_FLASHCARD,
});

const responseDeleteFlashcard = (id) => ({
    type: RESPONSE_DELETE_FLASHCARD,
    id
});

const setDeleteFlashcardError = () => ({
    type: SET_DELETE_FLASHCARD_ERROR
});

