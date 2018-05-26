import {combineReducers} from 'redux';
import manageCategories from './manageCategories';
import manageFlashcards from './manageFlashcards';
import learning from './learning';
import dialog from './dialog';
import login from './login';
import admin from './admin';

const app = combineReducers({
    learning,
    login,
    manageCategories,
    manageFlashcards,
    admin
});

export default app;