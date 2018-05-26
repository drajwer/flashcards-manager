import MainPage from "../containers/MainPage";
import SelectCategoryPage from "../containers/SelectCategoryPage";
import SelectModePage from "../containers/SelectModePage";
import LearningFlashcardPage from "../containers/LearningFlashcardPage";
import ManageCategoriesMainPage from "../containers/ManageCategoriesMainPage";
import ManageFlashcardsMainPage from "../containers/ManageFlashcardsMainPage";
import ManageCategoryPage from "../containers/ManageCategoryPage";
import ManageFlashcardPage from "../containers/ManageFlashcardPage";
import LoginPage from '../containers/LoginPage';
import RegisterPage from '../containers/RegisterPage';
import AdminMainPage from '../containers/AdminMainPage';
import routeLabels from './routeLabels';
import AccountPage from "../containers/AccountPage";

export const routeComponents = {
    'MAIN_PAGE': {
        component: MainPage,
    },
    'SELECT_CATEGORY_PAGE': {
        component: SelectCategoryPage,
    },
    'SELECT_MODE_PAGE': {
        component: SelectModePage,
    },
    'LEARNING_FLASHCARD_PAGE': {
        component: LearningFlashcardPage,
    },
    'MANAGE_CATEGORIES_MAIN_PAGE': {
        component: ManageCategoriesMainPage
    },
    'MANAGE_FLASHCARDS_MAIN_PAGE': {
        component: ManageFlashcardsMainPage
    },
    'MANAGE_CATEGORY_PAGE': {
        component: ManageCategoryPage
    },
    'MANAGE_FLASHCARD_PAGE': {
        component: ManageFlashcardPage
    },
    'LOGIN_PAGE': {
        component: LoginPage
    },
    'REGISTER_PAGE': {
        component: RegisterPage
    },
    'ACCOUNT_PAGE': {
        component: AccountPage
    },
    'ADMIN_MAIN_PAGE' : {
        component: AdminMainPage
    }
};
