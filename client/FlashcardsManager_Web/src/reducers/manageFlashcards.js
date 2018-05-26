const initialState = {
    isFetching: false,
    isInvalid: false,
    showToast: false,
    message: '',
    flashcards: [],
    flashcardId: -1,
};
import {
    RECEIVE_OWN_FLASHCARDS, REQUEST_OWN_FLASHCARDS, SET_FETCH_ERROR_MANAGE_FLASHCARDS, SELECT_OWN_FLASHCARD,
    REQUEST_ADD_FLASHCARD, REQUEST_DELETE_FLASHCARD,
    RESPONSE_ADD_FLASHCARD, RESPONSE_DELETE_FLASHCARD,
    SET_ADD_FLASHCARD_ERROR, SET_DELETE_FLASHCARD_ERROR, RESET_TOAST
} from '../actions/manageFlashcards';
import { RESET_STATE } from '../actions';

const manageFlashcards = (state = initialState, action) => {
    switch (action.type) {
        case RESET_STATE:
            return {
                ...initialState
            };
        case SELECT_OWN_FLASHCARD:
            return {
                ...state,
                flashcardId: action.flashcardId
            };
        case REQUEST_DELETE_FLASHCARD:
        case REQUEST_ADD_FLASHCARD:
        case REQUEST_OWN_FLASHCARDS:
            return {
                ...state,
                isFetching: true,
                isInvalid: false,
            };
        case RESPONSE_DELETE_FLASHCARD:
            return responseDeleteFlashcard(state, action.id);
        case RESPONSE_ADD_FLASHCARD:
            return responseAddFlashcard(state, action.flashcard);
        case RECEIVE_OWN_FLASHCARDS:
            return {
                ...state,
                flashcards: action.flashcards,
                isFetching: false
            };
        case RESET_TOAST:
            return {
                ...state,
                showToast: false,
                message: ''
            };
        case SET_ADD_FLASHCARD_ERROR:
        case SET_DELETE_FLASHCARD_ERROR:
        case SET_FETCH_ERROR_MANAGE_FLASHCARDS:
            return {
                ...state,
                isInvalid: true
            };

        default:
            return state;
    }
}

const responseAddFlashcard = (state, flashcard) => {
    let flashcards = state.flashcards;
    flashcards.push(flashcard);
    return {
        ...state,
        isFetching: false,
        showToast: true,
        message: 'Pomyślnie dodano fiszkę',
        flashcards: flashcards
    };
};

const responseDeleteFlashcard = (state, id) => {
    let flashcards = state.flashcards;
    let toDel = state.flashcards.find(c => c.id === id);
    const index = flashcards.indexOf(toDel);
    flashcards.splice(index, 1);
    return {
        ...state,
        isFetching: false,
        showToast: true,
        message: 'Pomyślnie usunięto kategorię',
        flashcards: flashcards
    };
};


export default manageFlashcards;