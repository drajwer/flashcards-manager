import { v4 as generateId } from 'node-uuid';

import { queryWeather } from '../api';
import { getLearningFlashcardsApiCall, getCategoriesApiCall, proceedFlashcardApiCall } from '../api/learningApi';
export const REQUEST_CATEGORIES = 'REQUEST_CATEGORIES';
export const RECEIVE_CATEGORIES = 'RECEIVE_CATEGORIES';

export const SELECT_CATEGORY = 'SELECT_CATEGORY';
export const SELECT_MODE = 'SELECT_MODE';
export const REQUEST_LEARNING_FLASHCARDS = 'REQUEST_LEARNING_FLASHCARDS';
export const RECEIVE_LEARNING_FLASHCARDS = 'RECEIVE_LEARNING_FLASHCARDS';
export const SET_FETCH_LEARNING_FLASHCARDS_ERROR = 'SET_FETCH_LEARNING_FLASHCARDS_ERROR';

export const REQUEST_PROCEED_FLASHCARD = 'REQUEST_PROCEED_FLASHCARD';
export const SET_NEXT_FLASHCARD = 'SET_NEXT_FLASHCARD';
export const SET_FETCH_ERROR = 'SET_FETCH_ERROR';

export const OPEN_DIALOG = 'OPEN_DIALOG';
export const CLOSE_DIALOG = 'CLOSE_DIALOG';

export const RESET_STATE = 'RESET_STATE';

export const resetState = () => ({
  type: RESET_STATE
});

export const requestCategories = () => ({
  type: REQUEST_CATEGORIES,
});

export const receiveCategories = (categories) => ({
  type: RECEIVE_CATEGORIES,
  categories
});

export const selectCategory = id => ({
  type: SELECT_CATEGORY,
  categoryId: id,
});

export const selectMode = mode => ({
  type: SELECT_MODE,
  mode
});

export const requestLearningFlashcards = () => ({
  type: REQUEST_LEARNING_FLASHCARDS,
});

export const receiveLearningFlashcards = (flashcards) => ({
  type: RECEIVE_LEARNING_FLASHCARDS,
  flashcards
});

export const setFetchError = () => ({
  type: SET_FETCH_ERROR,
});

export const setNextFlashcard = () => ({
  type: SET_NEXT_FLASHCARD,
});

export const getCategories = () => {
  return (dispatch, getState) => {
    dispatch(requestCategories());
    getCategoriesApiCall(getState().login.bearerToken)
      .then((categories) => dispatch(receiveCategories(categories)))
      .catch(() => dispatch(setFetchError()));
  };
};


export const startLearning = (mode) => {
  return (dispatch, getState) => {
    const categoryId = getState().learning.categoryId;
    dispatch(selectMode(mode));
    dispatch(requestLearningFlashcards());
    getLearningFlashcardsApiCall(categoryId, mode, getState().login.bearerToken)
      .then((flashcards) => dispatch(receiveLearningFlashcards(flashcards)))
      .catch(() => dispatch(setFetchError()));
  };
}

export const proceedFlashcard = result => {
  return (dispatch, getState) => {
    const state = getState().learning;
    const id = state.flashcards[state.flashcardIndex].id;

    dispatch(setNextFlashcard());
    proceedFlashcardApiCall(id, result, getState().login.bearerToken)
      .catch(() => dispatch(setFetchError()));
  };
};
