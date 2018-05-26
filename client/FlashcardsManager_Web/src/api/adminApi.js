import { getApiCall, putApiCall } from './index';
import {apiUrls} from './apiUrls';

export function getAdminCategoriesApiCall(bearerToken) {
    const url = apiUrls.adminCategoriesEndpoint;
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

export function acceptCategoryApiCall(category, bearerToken) {
    const url = apiUrls.adminAcceptEndpoint + `/${category.id}`;
    const data = category;
    
    return putApiCall(url, data, bearerToken);
}

export function rejectCategoryApiCall(category, bearerToken) {
    const url = apiUrls.adminRejectEndpoint + `/${category.id}`;
    const data = category;
    
    return putApiCall(url, data, bearerToken);
}