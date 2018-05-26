const initialState = {
    isFetching: false,
    isInvalid: false,
    categories: [],
    categoryId: -1,
    mode: -1,
    flashcards: [],
    flashcardIndex: -1,
};
import {
    SELECT_CATEGORY, REQUEST_CATEGORIES, SELECT_MODE, REQUEST_LEARNING_FLASHCARDS,
    RECEIVE_LEARNING_FLASHCARDS, REQUEST_PROCEED_FLASHCARD, SET_NEXT_FLASHCARD, SET_FETCH_ERROR, RECEIVE_CATEGORIES
} from '../actions';
import { RESET_STATE } from '../actions';

const learning = (state = initialState, action) => {
    switch (action.type) {
        case RESET_STATE:
            return {
                ...initialState
            };
        case SELECT_CATEGORY:
            return {
                ...state,
                categoryId: action.categoryId
            };
        case SELECT_MODE:
            return {
                ...state,
                mode: action.mode
            };
        case REQUEST_CATEGORIES:
        case REQUEST_PROCEED_FLASHCARD:
        case REQUEST_LEARNING_FLASHCARDS: {
            return {
                ...state,
                isFetching: true
            };
        }
        case RECEIVE_LEARNING_FLASHCARDS:
            return {
                ...state,
                flashcards: action.flashcards,
                flashcardIndex: 0,
                isFetching: false
            };
        case RECEIVE_CATEGORIES:
            return {
                ...state,
                categories: action.categories,
                isFetching: false
            };
        case SET_NEXT_FLASHCARD:
            return {
                ...state,
                flashcardIndex: state.flashcardIndex + 1
            }
        case SET_FETCH_ERROR:
            return {
                ...state,
                isInvalid: true
            }

        default:
            return state;
    }
}

export default learning;