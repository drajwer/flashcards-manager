const host = "https://flashcards-manager.azurewebsites.net";//"https://localhost:44380";//
const apiUrl = host + "/api";
const admin = apiUrl + "/administrator";

export const apiUrls = {
    host: host,
    apiUrl: host + "/api",
    userEndpoint: apiUrl + "/users",
    categoriesEndpoint: apiUrl + "/categories",
    ownCategoriesEndpoint: apiUrl + "/categories/mine",

    adminCategoriesEndpoint: admin + '/categories',
    adminAcceptEndpoint: admin + '/confirm',
    adminRejectEndpoint: admin + '/reject',
    
    flashcardsEndpoint: apiUrl + "/flashcards",
    flashcardsOfOneCategory: apiUrl + "/flashcards/category",
    learningFlashcardsEndpoint: apiUrl + "/learning/flashcards",
    learningResultEndpoint: apiUrl + "/learning/result",
    clientName: 'wpf',
    clientSecret: 'flashcardsSecret',
    tokenEndpoint: host + "/connect/token",
    scope: "flashcardsScope",
    registerAction: apiUrl + "/account/register",
};
