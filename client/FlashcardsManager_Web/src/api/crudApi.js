import { getApiCall, postApiCall, postWithReturnApiCall, putApiCall, deleteApiCall } from './index';
import { apiUrls} from './apiUrls';

export function getOwnCategoriesApiCall(bearerToken) {
    const url = apiUrls.ownCategoriesEndpoint;
    return getApiCall(url, bearerToken)
        .then(json => json.map(arrayMember => {
            return {
                id: arrayMember.id,
                name: arrayMember.name,
                availability: arrayMember.availability
            };
        })
        );
}

export function addCategoryApiCall(categoryName, availability, bearerToken) {
    //return new Promise((resolve, reject) => resolve());
    const url = apiUrls.categoriesEndpoint;
    const data = {
        name: categoryName,
        availability
    };
    return postWithReturnApiCall(url, data, bearerToken);
}

export function updateCategoryApiCall(category, bearerToken) {
    //return new Promise((resolve, reject) => resolve());
    const url = apiUrls.categoriesEndpoint + `/${category.id}`;
    const data = category;
    
    return putApiCall(url, data, bearerToken);
}

export function deleteCategoryApiCall(id, bearerToken) {
    const url = apiUrls.categoriesEndpoint + `/${id}`;

    return deleteApiCall(url, bearerToken);
}

export function getOwnFlashcardsApiCall(id, bearerToken) {
    const url = apiUrls.flashcardsOfOneCategory + `/${id}`;
    return getApiCall(url, bearerToken)
        .then(json => json.map(arrayMember => {
            return {
                id: arrayMember.id,
                key: arrayMember.key,
                value: arrayMember.value,
                keyDescription: arrayMember.keyDescription,
                valueDescription: arrayMember.valueDescription
            };
        })
        );
}

export function addFlashcardApiCall(key, value, keyDesc, valueDesc, categoryId, bearerToken) {
    //return new Promise((resolve, reject) => resolve());
    const url = apiUrls.flashcardsEndpoint;
    const data = {
        key,
        value,
        keyDescription: keyDesc,
        valueDescription: valueDesc,
        categoryId
    };
    return postWithReturnApiCall(url, data, bearerToken);
}

export function deleteFlashcardApiCall(id, bearerToken) {
    const url = apiUrls.flashcardsEndpoint + `/${id}`;

    return deleteApiCall(url, bearerToken);
}